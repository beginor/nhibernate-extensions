-- Table: public.test_table

-- DROP TABLE public.test_table;

CREATE TABLE public.test_table
(
    id integer NOT NULL DEFAULT nextval('test_table_id_seq'::regclass),
    name character varying(32) COLLATE pg_catalog."default" NOT NULL,
    tags character varying(32)[] COLLATE pg_catalog."default",
    json_field json,
    jsonb_field jsonb,
    update_time timestamp without time zone,
    int32_arr integer[],
    int16_arr smallint[],
    int64_arr bigint[],
    real_arr real[],
    double_arr double precision[],
    bool_arr boolean[],
    CONSTRAINT pk_test_table PRIMARY KEY (id)
)
WITH (
    OIDS = FALSE
)
TABLESPACE pg_default;

ALTER TABLE public.test_table
    OWNER to postgres;
COMMENT ON TABLE public.test_table
    IS 'test table';