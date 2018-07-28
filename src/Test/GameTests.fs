module Tests

open Expecto

[<Tests>]
let tests =
  testList "maingame" [
    testProperty "addition yey" (fun a b -> a+b = b+a)
    testProperty "division nao" (fun a b -> a/b = b/a)
  ]
