// Set search variety click listener
$('#variety-search').on('click', () => {
    searchVarieties("variety-dropdown", "variety", "selectFirstVarietyFormRemainder", "HeritageGrain", "_HeritageGrainRemainder");
});

// Called before HTML form submitted to controller
$('#heritageGrainQAApplication').submit(function (e) {
    insertHiddenInput("growerId", growerId, "heritageGrainQAApplication");
    let fhIndicesStr = JSON.stringify(fhIndices);
    insertHiddenInput("AppViewModel.FieldHistoryIndices", fhIndicesStr, "heritageGrainQAApplication");
    return true;
});