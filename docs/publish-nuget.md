# 使用 Trusted Publishing 发布 NuGet 包

nuget.org 已不再推荐长期 API key，改用 Trusted Publishing：CI 用 GitHub OIDC token 换取 1 小时有效的临时 key 再推送，仓库里不需要保存任何密钥。

本文是通用步骤，其它项目照抄即可，只需替换仓库名、包名和版本号。

## 1. 准备发布元数据

包版本由环境变量注入，`release.env` 是唯一来源，本地打包脚本和 CI 工作流都 source 它：

```bash
export PACKAGE_VERSION="10.0.6"
export PACKAGE_RELEASE_NOTES="Update Microsoft packages to 10.0.15"
export PACKAGE_TAGS="nhibernate, npgsql, dotnet"
export PACKAGE_PROJECT_URL="https://github.com/beginor/nhibernate-extensions"
```

`csproj` 里引用这些变量：

```xml
<PackageVersion>$(PACKAGE_VERSION)</PackageVersion>
<PackageReleaseNotes>$(PACKAGE_RELEASE_NOTES)</PackageReleaseNotes>
<PackageTags>$(PACKAGE_TAGS)</PackageTags>
<PackageProjectUrl>$(PACKAGE_PROJECT_URL)</PackageProjectUrl>
```

变量必须 `export`，否则 `dotnet pack` 读不到，会退化成默认版本 `1.0.0`。

## 2. 在 nuget.org 创建策略

登录 nuget.org → 用户菜单 → **Trusted Publishing** → Create：

| 字段 | 值 |
| --- | --- |
| Package owner | 你的账号或组织（必须是包的 owner） |
| Repository Owner | GitHub 用户名或组织，如 `beginor` |
| Repository | 仓库名，如 `nhibernate-extensions` |
| Workflow File | 只填文件名，如 `publish-nuget.yml`，不要带 `.github/workflows/` |
| Environment | 留空；填了就必须在 workflow 里写同样的 `environment:` |
| Scopes | Push new packages and package versions |
| Glob | 包 ID 模式，如 `NHibernate.Extensions.*` |

Workflow File 和 Environment 必须与工作流完全一致，否则换取 key 时报 `No matching trust policy`。

## 3. 在 GitHub 添加变量

Settings → Secrets and variables → Actions → Variables → New repository variable：

| 名称 | 值 |
| --- | --- |
| `NUGET_USER` | nuget.org 的用户名（profile name，不是邮箱） |

## 4. 工作流

`.github/workflows/publish-nuget.yml`：

```yaml
name: publish-nuget

on:
  push:
    tags: ["[0-9]*"]
  workflow_dispatch:

jobs:
  publish:
    runs-on: ubuntu-latest
    permissions:
      contents: read
      id-token: write # 换取 OIDC token 必需
    steps:
      - uses: actions/checkout@v7
      - uses: actions/setup-dotnet@v6
        with:
          dotnet-version: 10.0.x

      - name: Check tag matches release.env
        if: startsWith(github.ref, 'refs/tags/')
        run: |
          source ./release.env
          test "refs/tags/$PACKAGE_VERSION" = "$GITHUB_REF" \
            || { echo "tag $GITHUB_REF does not match PACKAGE_VERSION=$PACKAGE_VERSION"; exit 1; }

      - name: Pack
        run: |
          source ./release.env
          for proj in src/*/*.csproj; do
            dotnet pack "$proj" -c Release --output ./nupkgs/
          done

      - name: NuGet login (OIDC -> temporary API key)
        id: login
        uses: NuGet/login@v1
        with:
          user: ${{ vars.NUGET_USER }}

      - name: Push packages
        run: |
          dotnet nuget push ./nupkgs/*.nupkg \
            --source https://api.nuget.org/v3/index.json \
            --api-key "${{ steps.login.outputs.NUGET_API_KEY }}" \
            --skip-duplicate
```

要点：

- `id-token: write` 只给发布 job，构建/测试 job 不需要；
- `NuGet/login` 要放在 `dotnet nuget push` 之前，临时 key 只有 1 小时；
- 直接把 OIDC token 当 `--api-key` 用会 403，必须先换 key；
- 用 `--skip-duplicate`，重复触发不会失败。

## 5. 发布流程

1. 更新 `CHANGELOG.md`；
2. 修改 `release.env` 的 `PACKAGE_VERSION` 和 `PACKAGE_RELEASE_NOTES`；
3. 本地打包验证（产物在 `nupkgs/`，不推送）：

```bash
source ./release.env
dotnet pack src/Your.Package -c Release --output ./nupkgs/
```

4. 提交后打标签并推送，标签触发工作流：

```bash
git tag 10.0.6
git push origin master 10.0.6
```

5. 在 Actions 里确认 run 成功，再到 nuget.org 确认包已出现（验证和索引最长约 1 小时）。

## 常见问题

| 现象 | 原因 |
| --- | --- |
| `No matching trust policy owned by user '...'` | `NUGET_USER` 不是策略所属账号，或策略的 Workflow/Environment 与工作流不一致 |
| 403 `The specified API key has expired` | 还在用旧的长期 API key，本地 `dotnet nuget push` 只能走 API key，CI 才支持 Trusted Publishing |
| 包版本变成 `1.0.0` | `release.env` 里的变量没有 `export` |
| 策略显示 Pending | 私有仓库的策略初始只有 7 天有效期，首次成功换取 key 后永久激活 |
