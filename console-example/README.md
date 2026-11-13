# NEW Forex console example (AI-assisted)

This is a **new educational Visual Basic console example**, created with AI assistance from an incomplete source excerpt. It is **not a restoration** of the original Windows Forms app; the original form designer and complete event handlers are unavailable. It does not provide transaction services.

## Origin and assumptions

Based on [`stuvb.VB.txt` in katumwaarnold/KatumwaArnold0493](https://github.com/katumwaarnold/KatumwaArnold0493/blob/eca35035d000c44a156e9399ab8869f2bbd3589f/stuvb.VB.txt), commit `eca35035d000c44a156e9399ab8869f2bbd3589f` (blob `42664c323a743ab7baa8b1f68609f178f0755595`). The excerpt gives **1 Chinese yuan = 540 UGX** and **1 UAE dirham = 110 UGX**, with no date or source. These hardcoded figures are **undated, illustrative only, not verified or live**, and must **not be used for transactions**.

The yuan conversions use the excerpt's division and multiplication formulas. The excerpt is incomplete: both dirham conversion calculations are **inferred**, not recovered. This example divides UGX by 110 for dirhams and multiplies dirhams by 110 for UGX. It assumes the same rate in both directions, no fees and no spread. These assumptions are not evidence of actual bureau practices.

## Run and test

Requires the .NET 8 SDK. From this directory:

```sh
dotnet build ForexConverter.vbproj
dotnet run --project ForexConverter.vbproj
dotnet run --project test/ForexTests.vbproj
```

The last command runs a self-contained test harness, not a `dotnet test` framework. It checks the four directions, input validation, locale behavior, negative amounts, overflow and display rounding. Input uses the current locale's decimal separator; grouping separators are rejected. Results display two decimal places: small results may display as zero, and round trips using displayed values may differ due to rounding. This is not an accounting specification.

To recreate the original app, obtain the complete Visual Studio project, form designer, resources and handlers, and confirm all requirements separately.
