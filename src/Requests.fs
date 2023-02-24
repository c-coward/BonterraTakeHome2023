namespace Challenge

open Oryx
open Oryx.ThothJsonNet.ResponseReader

open Challenge.Logic
open Challenge.Decoders

/// Contains functions for building requests to the NGP API
module Requests =

    /// Create an initial GET request with the given HttpClient
    let openRequest client =
        httpRequest |> GET |> withHttpClient client

    /// Authenticate an HTTP request with the given API key
    let authenticate (key: string) = withHeader ("apiKey", key)

    /// Serializes the given Endpoint and uses it as the url for the HTTP request
    let withEndpoint (endpoint: Endpoint) = serialize endpoint |> withUrl

    /// Retrieve a list of all EmailMessages from the base endpoint.
    let requestShallowEmailResponseList request =
        request
        |> withEndpoint baseEndpoint
        |> fetch
        |> json shallowEmailResponseListDecoder

    /// Retrieve information about an email with the given `emailId`.
    let requestEmailResponse emailId request =
        request
        |> withEndpoint (withEmailId emailId)
        |> fetch
        |> json emailResponseDecoder

    /// Retrieve information about the `EmailVariant` associated with the given `emailId` and `variantId`.
    let requestVariantResponse emailId variantId request =
        request
        |> withEndpoint (withVariantId emailId variantId)
        |> fetch
        |> json variantResponseDecoder

    /// Retrieve the name of the top variant associated with the given `emailId`.
    let requestTopVariant emailId request =
        http {
            let! emailResponse = requestEmailResponse emailId request

            let variantIds =
                emailResponse.variants |> List.map (fun v -> v.emailMessageVariantId)

            let! variants = http.For(variantIds, (fun variantId -> requestVariantResponse emailId variantId request))

            return findTopVariantName variants
        }

    /// Retrieve information about an email with the given `emailId`, in a more useful format.
    let requestEmailData emailId request =
        http {
            let! emailResponse = requestEmailResponse emailId request
            let! topVariant = requestTopVariant emailId request

            return getEmailData emailResponse topVariant
        }

    /// Retrieve information about all emails in the system, in a more useful format.
    let requestAllEmailDatas request =
        http {
            let! shallowResponses = requestShallowEmailResponseList request
            let emailIds = shallowResponses |> List.map (fun e -> e.emailMessageId)

            return! http.For(emailIds, (fun id -> requestEmailData id request))
        }
