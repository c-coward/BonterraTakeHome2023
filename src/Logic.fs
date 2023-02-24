namespace Challenge

/// Contains literals, types, and functions pertaining to the core logic of the application.
module Logic =

    [<Literal>]
    let Url = "https://api.myngp.com/v2/BroadcastEmails"

    /// Represents a URL to be consumed by the NGP API.
    type Endpoint =
        | Base of string
        | WithEmailId of string * int
        | WithVariantId of string * int * int

    /// The base endpoint, on which all others are built.
    let baseEndpoint = Base Url
    /// Construct an endpoint for making API calls related to a given `emailId`.
    let withEmailId emailId = WithEmailId(Url, emailId)
    /// Construct an endpoint for making API calls related to a given `emailId` and `variantId`.
    let withVariantId emailId variantId = WithVariantId(Url, emailId, variantId)

    /// Convert an `Endpoint` into a raw string
    let serialize (endpoint: Endpoint) : string =
        match endpoint with
        | Base url -> url
        | WithEmailId(url, emailId) -> $"{url}/{emailId}?$expand=statistics"
        | WithVariantId(url, emailId, variantId) -> $"{url}/{emailId}/variants/{variantId}?$expand=statistics"

    type EmailVariant =
        { emailMessageVariantId: int
          name: string
          subject: string }

    type EmailStatistics =
        { recipients: int
          opens: int
          clicks: int
          unsubscribes: int
          bounces: int }

    type ShallowEmailResponse = { emailMessageId: int; name: string }

    type EmailResponse =
        { emailMessageId: int
          name: string
          variants: EmailVariant list
          statistics: EmailStatistics }

    type VariantResponse =
        { emailMessageId: int
          emailMessageVariantId: int
          name: string
          subject: string
          statistics: EmailStatistics }

    type EmailData =
        { id: int
          name: string
          recipients: int
          opens: int
          clicks: int
          unsubscribes: int
          bounces: int
          topVariant: string }

    /// Select the variant with the largest `opens` statistic from the given list, and return its name.
    /// If the input list is empty, returns an empty string.
    let findTopVariantName (variants: VariantResponse list) : string =
        match variants with
        | [] -> ""
        | _ -> variants |> List.maxBy (fun v -> v.statistics.opens) |> (fun x -> x.name)

    /// Converts an `EmailResponse` returned by the API into a more useful representation.
    let getEmailData (email: EmailResponse) (variantName: string) : EmailData =
        { id = email.emailMessageId
          name = email.name
          recipients = email.statistics.recipients
          opens = email.statistics.opens
          clicks = email.statistics.clicks
          unsubscribes = email.statistics.unsubscribes
          bounces = email.statistics.bounces
          topVariant = variantName }

    [<Literal>]
    let CsvHeader =
        "Email Message ID,Email Name,Recipients,Opens,Clicks,Unsubscribes,Bounces,Top Variant"

    let emailDataToCsvRow (email: EmailData) =
        $"{email.id},{email.name},{email.recipients},{email.opens},{email.clicks},{email.unsubscribes},{email.bounces},{email.topVariant}"
