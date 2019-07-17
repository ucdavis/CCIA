// Set search variety click listener
// Located in seed.js because the callback of selecting a variety changes depending on App type
$('#variety-search').on('click', () => {
    searchVarieties("variety-dropdown", "variety", "selectFirstVarietyFormRemainder", "HempFromSeed", "_HempFromSeedRemainder");
});

// Called before HTML form submitted to controller
$('#hempFromSeedApplication').submit(function (e) {
    insertHiddenInput("growerId", growerId, "hempFromSeedApplication");
    let fhIndicesStr = JSON.stringify(fhIndices);
    insertHiddenInput("AppViewModel.FieldHistoryIndices", fhIndicesStr, "hempFromSeedApplication");
    return true;
});

function selectPs2Variety(varietyId, varietyName) {
    fillVarietyNameAndId("ps2-variety", "ps2-variety-id", varietyId, varietyName);
}