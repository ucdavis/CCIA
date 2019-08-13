// Set search variety click listener
$('#variety-search').on('click', () => {
    searchVarieties("variety-dropdown", "variety", "selectFirstVarietyFormRemainder")
});

$('#seedApplication').submit(function (e) {
    insertHiddenInput("growerId", growerId, "seedApplication");

    // Insert hidden input for fhEntryId to let the server know how many fieldhistory records we're submitting
    // Used in order to aid server-side validation on re-rendering of form
    insertHiddenInput("fhEntryId", fhEntryId, "seedApplication");
    return true;
});

function selectPs2Variety(varietyId, varietyName) {
    fillVarietyNameAndId("ps2-variety", "ps2-variety-id", varietyId, varietyName);
}