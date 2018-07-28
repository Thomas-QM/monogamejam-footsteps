module Game.Update

open Game
open Model

let mailbox:MailboxProcessor<Message> = MailboxProcessor.Start (fun x -> async {
    match! x.Receive() with
        | IntoGame -> ()
    ()
})