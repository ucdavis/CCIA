// Set search variety click listener
$('#variety-search').on('click', () => {
    searchVarieties("variety-dropdown", "variety", "selectFirstVariety")
});

$("#potatoApplication").submit(function (e) {
    insertHiddenInput("growerId", growerId, "potatoApplication");
    let fhIndicesStr = JSON.stringify(fhIndices);
    insertHiddenInput("AppViewModel.FieldHistoryIndices", fhIndicesStr, "potatoApplication");
    return true;
});