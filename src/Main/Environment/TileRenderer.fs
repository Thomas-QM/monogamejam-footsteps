module Environment.TileSelector

open Environment
open Model
open Microsoft.Xna.Framework
open Microsoft.Xna.Framework.Graphics
open Comora

let rand = new System.Random()

let SelectTile ({Tiles=env}:Environment) x y =
    match Map.tryFind (x,y) env with
        | Some x -> env, x
        | None ->
            let tile:ClimateTile = if rand.Next(0,2) = 1 then Debug else Hot Sand
            Map.add (x,y) tile env, tile //TODO: Separate tile rendering and generation, make a tile array to store tile positions which update on update as well

let tilesize = 10

let UpdateTiles (graphics:GraphicsDeviceManager) (cam:ICamera) (env:Environment) =
    let width = cam.Width |> int
    let height = cam.Height |> int

    let tileheight = width/(tilesize*2)
    let tilewidth = width/tilesize

    let extratiles = 2

    let campos = cam.Position-Vector2(cam.Width/float32 2, cam.Height/float32 2)
    let camx, camy = campos.X-float32 (extratiles*tilewidth), campos.Y-float32 (extratiles*tileheight)
    let tilex, tiley = (camx |> int)/tilewidth, (camy |> int)/tileheight
    let off = Vector2(camx%float32 tilewidth, camy%float32 tileheight)

    let rows = height/tileheight
    let row = [0 .. (tilesize*2)+(extratiles*2)]
    let tiles =
        [0 .. rows+(extratiles*2)]
        |> List.collect (fun y -> row |> List.map (fun x -> (tileheight*y)+(x%2) * (tileheight/2),
                                                            x*tilewidth/2))

    tiles |> List.fold (fun env (y,x) ->
        let newtiles, tile = SelectTile env ((x/tilewidth)+tilex) ((y/tileheight)+tiley)

        let vec = Vector2(camx-off.X+float32 x, camy-off.Y+(y-tilewidth/2 |> float32))
        let rendertile = {Position=vec-Vector2(float32 1); Width=tilewidth+1; Tile=tile} //+1 to fix antialiased edges breaking up tiles

        {env with Tiles=newtiles; RenderTiles=rendertile::env.RenderTiles}) {env with RenderTiles=[]}

let RenderTiles (graphics:GraphicsDeviceManager) ({RenderTiles=tiles}:Environment) content (sprites:SpriteBatch) =
    tiles |> List.iter (fun {Position=pos; Tile=tile; Width=width;} ->
        let tex = Map.find tile content
        sprites.Draw(tex, new Rectangle(int pos.X, int pos.Y, width, width), Color.White)
    )