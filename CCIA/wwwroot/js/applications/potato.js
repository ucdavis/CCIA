// Set search variety click listener
$('#variety-search').on('click', () => {
    searchVarieties("variety-dropdown", "variety", "selectFirstVariety")
});

$("#potatoApplication").submit(function (e) {
    insertHiddenInput("growerId", growerId, "potatoApplication");
    return true;
});

function selectFirstVariety(varietyId, varietyName) {
    // Original variety
    fillVarietyNameAndId("variety", "variety-id", varietyId, varietyName);
    // Planting Stock 1 Variety
    fillVarietyNameAndId("ps1-variety", "ps1-variety-id", varietyId, varietyName);
    showSpinner("variety-dropdown");
}