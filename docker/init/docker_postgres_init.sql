
CREATE TABLE IF NOT EXISTS "outboxes"
(
    "Id" uuid NOT NULL,
    "Published" boolean NOT NULL,
    "Event" text COLLATE pg_catalog."default" NOT NULL,
    "PublishedAt" timestamp without time zone,
    "Type" text COLLATE pg_catalog."default" NOT NULL,
    "CreatedAt" timestamp without time zone NOT NULL,
    "UpdatedAt" timestamp without time zone,
    CONSTRAINT "PK_Outboxes" PRIMARY KEY ("Id")
);

CREATE TABLE IF NOT EXISTS "activies"
(
    "Id" uuid NOT NULL,
    "Name" text COLLATE pg_catalog."default" NOT NULL,
    "Description" text COLLATE pg_catalog."default" NOT NULL,
    "DateEstimatedFinish" timestamp without time zone,
    "Priority" integer NOT NULL,
    "Status" integer NOT NULL,
    "Delayed" boolean NOT NULL,
    "CreatedAt" timestamp without time zone NOT NULL,
    "UpdatedAt" timestamp without time zone,
    CONSTRAINT "PK_Activies" PRIMARY KEY ("Id")
);

