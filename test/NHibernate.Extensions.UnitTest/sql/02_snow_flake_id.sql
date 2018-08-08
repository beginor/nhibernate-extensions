-- DROP SEQUENCE public.snow_flake_id_seq;

CREATE SEQUENCE public.snow_flake_id_seq;

ALTER SEQUENCE public.snow_flake_id_seq
    OWNER TO postgres;

-- FUNCTION: public.snow_flake_id()

-- DROP FUNCTION public.snow_flake_id();

CREATE OR REPLACE FUNCTION public.snow_flake_id()
    RETURNS bigint
    LANGUAGE 'sql'
    COST 100
    VOLATILE
AS $BODY$

SELECT (EXTRACT(EPOCH FROM CURRENT_TIMESTAMP) * 1000)::bigint * 1000000
  + 5 * 10000
  + nextval('public.snow_flake_id_seq') % 1000
  as snow_flake_id

$BODY$;

ALTER FUNCTION public.snow_flake_id()
    OWNER TO postgres;

COMMENT ON FUNCTION public.snow_flake_id()
    IS 'snow flake id ';

