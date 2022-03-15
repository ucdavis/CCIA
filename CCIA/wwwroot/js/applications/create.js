// Set search variety click listener
// Located in seed.js because the callback of selecting a variety changes depending on App type
$('#varietySearch').on('click', () => {
    searchVarieties("variety-dropdown", "variety", "selectFirstVarietyFormRemainder", "Seed", "SeedAppPartial");
});

// Called before HTML form submitted to controller
$('#seedApplication').submit(function (e) {
    insertHiddenInput("growerId", growerId, "seedApplication");
    let fhIndicesStr = JSON.stringify(fhIndices);
    insertHiddenInput("AppViewModel.FieldHistoryIndices", fhIndicesStr, "seedApplication");
    return true;
});

function selectPs2Variety(varietyId, varietyName) {
    fillVarietyNameAndId("ps2-variety", "ps2-variety-id", varietyId, varietyName);
}