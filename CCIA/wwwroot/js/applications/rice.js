// Set search variety click listener
// Located in seed.js because the callback of selecting a variety changes depending on App type
$('#variety-search').on('click', () => {
    searchVarieties("variety-dropdown", "variety", "selectFirstVarietyFormRemainder", "Rice", "_RiceRemainder");
});

// Called before HTML form submitted to controller
$('#riceQAApplication').submit(function (e) {
    insertHiddenInput("growerId", growerId, "riceQAApplication");
    let fhIndicesStr = JSON.stringify(fhIndices);
    insertHiddenInput("AppViewModel.FieldHistoryIndices", fhIndicesStr, "riceQAApplication");
    return true;
});