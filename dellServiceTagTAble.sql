CREATE TABLE `tbldellservicetags` (
	`id` INT(11) NOT NULL AUTO_INCREMENT,
	`country_lookup_code` VARCHAR(50) NULL DEFAULT NULL,
	`machine_description` VARCHAR(250) NULL DEFAULT NULL,
	`service_tag` VARCHAR(50) NULL DEFAULT NULL,
	`ship_date` DATETIME NULL DEFAULT NULL,
	PRIMARY KEY (`id`)
)
COLLATE='latin1_swedish_ci'
ENGINE=InnoDB
AUTO_INCREMENT=1
;
