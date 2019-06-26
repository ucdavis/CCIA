/**
 * @param  {} name
 * @param  {} insertVal
 * @param  {} formId
 */
function insertHiddenInput(name, insertVal, formId) {
    let inputInsert = document.createElement("input");
    inputInsert.setAttribute("type", "hidden");
    inputInsert.setAttribute("name", name);
    inputInsert.setAttribute("value", insertVal.toString());
    document.getElementById(formId).appendChild(inputInsert); 
}

function searchVarieties(dropdownId, varietyInputId, selectVarietyCallback) {
    // Display error if user tries to search for variety before selecting crop
    let crop = document.getElementsByName("CropId")[0];
    let cropText = crop.options[crop.selectedIndex].text;
    let cropId = crop.options[crop.selectedIndex].value;
    let varietyName = document.getElementById(varietyInputId).value;
    if (cropText === "") { 
        $("#cropAlert").modal('show');
        return;
    }

    if (varietyName === "") {
        $("#emptyVarAlert").modal('show');
        return;
    }

    let data = { 
        name: varietyName,
        cropId: cropId
    };
    let vs = document.getElementById(dropdownId);
    // Take text in input box and autofill input with the variety name that most closely matches it
    $.ajax({
        type: "GET",
        url: "/Application/FindVariety",
        data: data,
        success: function(res) {
            vs.innerHTML = "";
            if (res.length === 0) {
                $("#varAlert").modal('show');
                loadFormRemainder();
            }
            // Populate dropdown with list of varieties
            res.forEach((el) => {
                vs.innerHTML += `<a class="dropdown-item" onclick=${selectVarietyCallback}(this)
                                id=${el.varOffId} value=${el.varOffId}>${el.varOffName}</a>`;
            })
        },
        error: function(res) {
            console.log(res);
            alert("There was an error processing the request");
        }
    });
}
