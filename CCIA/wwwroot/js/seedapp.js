const spinner_div = `<div class="text-center"><div class="spinner-border text-center" role="status"><span class="sr-only">Loading...</span></div></div>`
var form = document.getElementById("seedApplication")
let appTypeId = 1;
// The index of the next Field History entry to be added to the form
let fhEntryId = 0;
// How many total Field History entries are in the form currently
let fhEntryCount = 0;
let secondPsRendered = false;
let fhIndices = [0, 0, 0];

// After successfully loading form remainder
// Set functionality for date time picker
$('#datetimepicker1').datetimepicker({
    format: 'MM/DD/YYYY'
});

// Set click listener for button to add second plantingstock
$('#add-second-ps').on('click', addSecondPs);

// Set click listener for button to add field history records
$('#add-fieldhistory').on('click', (e) => {
    addNewFieldHistory(e, appTypeId);
});

$('#seedApplication').submit (function(e) {
    insertHiddenInput("growerId", growerId, "seedApplication");
    
    // Insert hidden input for fhEntryId to let the server know how many fieldhistory records we're submitting
    // Used in order to aid server-side validation on re-rendering of form
    insertHiddenInput("fhEntryId", fhEntryId, "seedApplication");

    return true;
});

// Click listener for first variety search button
$('#variety-search').on('click', function(e) {
    e.preventDefault();
    searchVarieties("variety-dropdown", "variety", "selectVariety");
});

// Called when clicking a variety name in the first variety dropdown
function selectVariety(e) {
    // Set hidden input of variety id from selected variety
    document.getElementById("variety-id").value = e.getAttribute("value");
    // Set variety input text to be the selected variety from dropdown
    document.getElementById("variety").value = e.innerText;

    // Reset dropdown to be spinner for next search
    showSpinner("variety-dropdown");
    loadFormRemainder(e.getAttribute("value"), e.innerText);
}

// Called when clicking a variety name in the second planting stock variety dropdown
function selectPs2Variety(e) {
    // Set hidden input of selected variety's id
    document.getElementById("ps2-variety").value = e.innerHTML;
}

/* Load the rest of the form after selecting (or entering) a variety*/
function loadFormRemainder(varietyId, varietyName) {
    showSpinner("form-remainder");
    /* If something is inside form-remainder div, clear it out */
    $("#form-remainder")
        .load("/Application/GetPartial?folder=Seed&partialName=SeedAppPartial&orgId="+orgId+"&appTypeId="+appTypeId, (response, status, xhr) => {
                if ( status == "error" ) {
                    var msg = "Sorry, the following error occurred while loading the remainder of the form: ";
                    $( "#error" ).html( msg + xhr.status + " " + xhr.statusText );
                }
                else {
                    // Set text in planting stock 1's variety to be what we selected above
                    document.getElementById("ps1-variety").value = varietyName;
                    // Set hidden input for planting stock 1's variety id
                    document.getElementById("ps1-variety-id").value = varietyId;
                }
        });
}

// Takes a parent element as a parameter, and places a centered bootstrap spinner inside
function showSpinner(parentId) {
    $(`#${parentId}`)[0].innerHTML = spinner_div;
}

/* Adds red outline to invalid text boxes on loading after failed form submission */
$(document).ready(() => {
    $('.field-validation-error').parents('.form-group').addClass('has-error');
});

function addNewFieldHistory(e, appTypeId) {
    e.preventDefault();
    // Populate div with an additional field history partial
    fhEntryId = findAvailableFhIndex();
    if (fhEntryId == -1) {
        return;
    }
    let fh_entry_div = `#fh-entry-${fhEntryId}`;
    // Show the section
    document.getElementById("fh-entry-"+fhEntryId).classList.remove("hidden");
    showSpinner("fh-entry-"+fhEntryId);
    $(fh_entry_div)
        .load("/Application/GetPartial?folder=Shared&partialName=_FieldHistoryEntry&orgId="+orgId+"&appTypeId="+appTypeId+"&fhEntryId="+fhEntryId, (response, status, xhr) => {
                if ( status == "error" ) {
                    var msg = "Sorry, there was an error loading the remainder of this form: ";
                    $( "#error" ).html( msg + xhr.status + " " + xhr.statusText );
                }
                else {
                    // Mark that id as used
                    fhIndices[fhEntryId] = 1;

                    // Click handler for removing additional field history entry
                    document.getElementById(fhEntryId).onclick = (e) => {
                        e.preventDefault();
                        // Grabs ID from closest parent section -- the parent section containing the "X" button
                        let idToRemove = parseInt(e.target.closest("button").id);
                        // Hide the section
                        document.getElementById("fh-entry-"+idToRemove).classList.add("hidden");
                        // Mark this entry as unused
                        if (fhEntryCount === 3) {
                            // Re-enable button to add new entry
                            document.getElementById("add-fieldhistory").disabled = false;
                        }
                        idToRemove = parseInt(e.target.closest("button").id);
                        fhIndices[idToRemove] = 0;
                        fhEntryCount--;
                        removeFhEntryById(idToRemove);                              
                    }
                    fhEntryCount++;
                }
                // TODO: change to pull from model's MaxFieldHistoryEntries property instead of using "3"
                if (fhEntryCount === 3)
                {
                    document.getElementById("add-fieldhistory").disabled = true;
                }
        });
}

function findAvailableFhIndex() {
    for (let i=0; i<fhIndices.length; i++) {
        // 0 = free, 1 = used
        if (fhIndices[i] === 0) {
            return i;
        }
    }
    return -1;
}

function addSecondPs(e) {
    e.preventDefault();
    showSpinner("second-ps");
    let appTypeId = 1;
    if (secondPsRendered) {
        return;
    }
    // Populate div with second planting stocks partial
    $("#second-ps")
        .load("/Application/GetPartial?folder=Seed&partialName=_SecondPlantingStock&orgId="+orgId+"&appTypeId="+appTypeId, (response, status, xhr) => {
                if ( status == "error" ) {
                    var msg = "Sorry, there was an error loading the remainder of this form: ";
                    $( "#error" ).html( msg + xhr.status + " " + xhr.statusText );
                }     
                else {
                    // Click listener for first variety search button
                    $('#ps2-variety-search').on('click', function(e) {
                        e.preventDefault();
                        searchVarieties("ps2-variety-dropdown", "ps2-variety", "selectPs2Variety");
                    });

                    secondPsRendered = true;
                    let addSeconPsBtn = document.getElementById("add-second-ps");
                    addSeconPsBtn.disabled = true;

                    // Click handler for removing the second planting stocks record
                    document.getElementById("remove-ps2").onclick = (e) => {
                        secondPsRendered = false;
                        e.preventDefault();
                        removeSecondPSEntry();
                        addSeconPsBtn.disabled = false;
                    }
                }           
        });
}

// Removes contents of a field history section, while keeping the section tags intact
function removeFhEntryById(id) {
    let fhEntrySection = `#fh-entry-${id}`;
    $(fhEntrySection).empty();
}

function removeSecondPSEntry(){
    $("#second-ps").empty();
}