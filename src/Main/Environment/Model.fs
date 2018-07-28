module Environment.Model

type HotTile = Sand | StonePath | StoneWall
type MediumTile = Grass | GrassPath | DirtWall
type ColdTile = Snow | SnowPath

type ClimateTile = Hot of HotTile | Medium of MediumTile | Cold of ColdTile
type Environment = ClimateTile seq seq