-- Table: public.snow_flake_test

-- DROP TABLE public.snow_flake_test;

CREATE TABLE public.snow_flake_test
(
    id bigint NOT NULL DEFAULT snow_flake_id(),
    name character varying(32) COLLATE pg_catalog."default",
    CONSTRAINT snow_flake_test_pkey PRIMARY KEY (id)
)
WITH (
    OIDS = FALSE
)
TABLESPACE pg_default;

ALTER TABLE public.snow_flake_test
    OWNER to postgres;
COMMENT ON TABLE public.snow_flake_test
    IS 'snow flake test table';
