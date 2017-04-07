CREATE TABLE `tbldellservicetags` (
	`id` INT(11) NOT NULL AUTO_INCREMENT,
	`country_lookup_code` VARCHAR(50) NULL DEFAULT NULL,
	`machine_description` VARCHAR(250) NULL DEFAULT NULL,
	`service_tag` VARCHAR(20) NULL DEFAULT NULL,
	`express_service_code` VARCHAR(20) NULL DEFAULT NULL,
	`ship_date` DATETIME NULL DEFAULT NULL,
	`regulatory_model` VARCHAR(30) NULL DEFAULT NULL,
	`regulatory_type` VARCHAR(30) NULL DEFAULT NULL,
	PRIMARY KEY (`id`)
)
COLLATE='latin1_swedish_ci'
ENGINE=InnoDB
AUTO_INCREMENT=1
;

CREATE TABLE `tbldellservicetagcomponents` (
	`id` INT(11) NOT NULL AUTO_INCREMENT,
	`parent_id` INT(11) NOT NULL,
	`description` VARCHAR(250) NULL DEFAULT NULL,
	PRIMARY KEY (`id`)
)
COLLATE='latin1_swedish_ci'
ENGINE=InnoDB
AUTO_INCREMENT=1
;

CREATE TABLE `tbldellservicetagcomponentparts` (
	`id` INT(11) NOT NULL AUTO_INCREMENT,
	`parent_id` INT(11) NOT NULL,
	`partnumber` VARCHAR(10) NULL DEFAULT NULL,
	`quantity` INT(4) NULL DEFAULT NULL,
	`description` VARCHAR(250) NULL DEFAULT NULL,
	PRIMARY KEY (`id`)
)
COLLATE='latin1_swedish_ci'
ENGINE=InnoDB
AUTO_INCREMENT=1
;
CREATE TABLE `tbldellservicetagdrivers` (
	`id` INT(11) NOT NULL AUTO_INCREMENT,
	`parent_id` INT(11) NOT NULL,
	`component` VARCHAR(50) NULL DEFAULT NULL,
	`name` VARCHAR(100) NULL DEFAULT NULL,
	`file_name` VARCHAR(100) NULL DEFAULT NULL,
	`description` VARCHAR(100) NULL DEFAULT NULL,
	`version` VARCHAR(50) NULL DEFAULT NULL,
	`importance` VARCHAR(50) NULL DEFAULT NULL,
	`release_date` DATE NULL DEFAULT NULL,
	`last_updated` DATE NULL DEFAULT NULL,
	`download_link` VARCHAR(300) NULL DEFAULT NULL,
	PRIMARY KEY (`id`)
)
COLLATE='latin1_swedish_ci'
ENGINE=InnoDB
AUTO_INCREMENT=1
;

