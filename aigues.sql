CREATE TABLE ConsumAigua (
	Id SERIAL NOT NULL PRIMARY KEY,
	Anys INT NOT NULL,
	Codi_Comarca INT NOT NULL,
	Comarca VARCHAR(20) NOT NULL,
	Poblacio INT NOT NULL,
	Domestic_xarxa FLOAT NOT NULL,
	Activitats_economiques_i_fonts_propies FLOAT NOT NULL,
	Total FLOAT NOT NULL,
	Consum_domestic_per_capital FLOAT NOT NULL
);