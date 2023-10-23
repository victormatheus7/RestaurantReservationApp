CREATE TABLE "public"."role" (
	id int2 NOT NULL,
	"name" varchar(16) NOT NULL,
	CONSTRAINT role_pk PRIMARY KEY (id)
);

CREATE TABLE "public".location_preference (
	id smallint NOT NULL,
	"name" varchar(16) NOT NULL,
	CONSTRAINT location_preference_pk PRIMARY KEY (id)
);

CREATE TABLE "public"."user" (
	email varchar(254) NOT NULL,
	password_hash bytea NOT NULL,
	password_salt bytea NOT NULL,
	role_id smallint NOT NULL,
	CONSTRAINT user_pk PRIMARY KEY (email),
	CONSTRAINT user_fk FOREIGN KEY (role_id) REFERENCES "public"."role"(id)
);

CREATE TABLE "public".reservation (
	id uuid NOT NULL,
	creator_email varchar(254) NOT NULL,
	"date" timestamp NOT NULL,
	number_seats int NOT NULL,
	location_preference_id smallint NOT NULL,
	observation varchar(1000) NULL,
	CONSTRAINT reservation_pk PRIMARY KEY (id),
	CONSTRAINT reservation_fk_location_preference FOREIGN KEY (location_preference_id) REFERENCES "public".location_preference(id),
	CONSTRAINT reservation_fk_user FOREIGN KEY (creator_email) REFERENCES "public"."user"(email)
);

INSERT INTO "public"."role" (id, "name")
VALUES(0, 'Client');

INSERT INTO "public"."role" (id, "name")
VALUES(1, 'Admin');

INSERT INTO "public".location_preference (id, "name")
VALUES(0, 'Indoor');

INSERT INTO "public".location_preference (id, "name")
VALUES(1, 'Outdoor');