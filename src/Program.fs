// open CsvHelper
open System.IO
open System.Net.Http
open Oryx

open Challenge.Requests
open Challenge.Logic

module Program =

    let secretFile = "secret.txt"
    let key = File.ReadAllText secretFile

    let writeEmailDatasCsv request =
        http {
            let! datas = request |> requestAllEmailDatas

            using (new StreamWriter("EmailReport.csv")) (fun file ->
                file.Write(CsvHeader)
                file.WriteLine()

                for data in datas |> List.sortByDescending (fun e -> e.id) do
                    file.Write(emailDataToCsvRow data)
                    file.WriteLine())
        }

    [<EntryPoint>]
    let main _ =
        task {
            use client = new HttpClient()

            let! _ = openRequest client |> authenticate key |> writeEmailDatasCsv |> runAsync

            printfn "Email report complete, file is EmailReport.csv"
        }
        |> Async.AwaitTask
        |> Async.RunSynchronously

        0 // Return a normal exit code
