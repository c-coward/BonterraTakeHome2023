namespace Challenge

open Thoth.Json.Net

open Challenge.Logic

/// Contains decoder methods for parsing JSON responses into types defined by the application.
module Decoders =

    let shallowEmailResponseDecoder: Decoder<ShallowEmailResponse> =
        Decode.object (fun get ->
            { emailMessageId = get.Required.Field "emailMessageId" Decode.int
              name = get.Required.Field "name" Decode.string })

    let shallowEmailResponseListDecoder: Decoder<ShallowEmailResponse list> =
        Decode.object (fun get -> get.Required.At [ "items" ] <| Decode.list shallowEmailResponseDecoder)

    let emailVariantDecoder: Decoder<EmailVariant> =
        Decode.object (fun get ->
            { emailMessageVariantId = get.Required.Field "emailMessageVariantId" Decode.int
              name = get.Required.Field "name" Decode.string
              subject = get.Required.Field "subject" Decode.string })

    let emailStatisticsDecoder: Decoder<EmailStatistics> =
        Decode.object (fun get ->
            { bounces = get.Required.Field "bounces" Decode.int
              clicks = get.Required.Field "clicks" Decode.int
              opens = get.Required.Field "opens" Decode.int
              recipients = get.Required.Field "recipients" Decode.int
              unsubscribes = get.Required.Field "unsubscribes" Decode.int })

    let emailResponseDecoder: Decoder<EmailResponse> =
        Decode.object (fun get ->
            { emailMessageId = get.Required.Field "emailMessageId" Decode.int
              name = get.Required.Field "name" Decode.string
              variants = get.Required.Field "variants" (Decode.list emailVariantDecoder)
              statistics = get.Required.Field "statistics" emailStatisticsDecoder })

    let variantResponseDecoder: Decoder<VariantResponse> =
        Decode.object (fun get ->
            { emailMessageId = get.Required.Field "emailMessageId" Decode.int
              emailMessageVariantId = get.Required.Field "emailMessageVariantId" Decode.int
              name = get.Required.Field "name" Decode.string
              subject = get.Required.Field "subject" Decode.string
              statistics = get.Required.Field "statistics" emailStatisticsDecoder })
