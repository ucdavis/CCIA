function displayReady(init)
{
    var selectValue = $("#origin").val();
    if(selectValue == "Select Origin")
    {
        if(init != "init")
        {
            alert("Please select an origin before continuing");
        }                
        $("#InState").hide();
        $("#OOState").hide();
        $("#Buttons").hide();
    }
    if(selectValue == "Ca")
    {
        $("#InState").show();
        $("#OOState").hide();
        $("#Buttons").show();
    }
        if(selectValue == "OOS")
    {
        $("#InState").hide();
        $("#OOState").show();
        $("#Buttons").hide();
    }
}

function checkLoad()
{
    displayReady("init");           
    if($("#comp_ApplicantId").val() && $("#comp_ApplicantId").val().length > 0)
    {
        $("#ApplicantSearch").val($("#comp_ApplicantId").val());
        $("#selectCrop").show();

        if($("#comp_OfficialVarietyId").val() && $("#comp_OfficialVarietyId").val().length > 0)
        {                   
            $("#Variety").val($("#comp_OfficialVarietyId").val());
            $("#completeForm").show();
            $("#Buttons").show();
        }
    }
}

$(document).ready(function () {
    checkLoad();
    $('#comp_CountryOfOrigin').change(function () {                
        SetStatesForCountry();
    })
    $('#retrieveApplicantButton').click(function () {
        var searchTerm = $('#ApplicantSearch').val(); 
        $.ajax({
            method: 'GET',
            url: '/client/Seeds/GetApplicants',
            data: {
                search: searchTerm,
            }
        })
        .done(function(data){                    
            $('#results').html("");
            var s = '<table class="table"><tr><th>&nbsp;</th><th>Org Id</th><th>Applicant</th></tr>';
                $.each(data,
                    function(i) {
                        var item = data[i];
                        s += '<tr><td><input type="button" class="btn btn-primary" value="Select" onClick="selectApplicant(' + item.id + ')"></input></td>';
                        s += '<td>' + item.id + '</td>';
                        s += '<td>' + item.name + '</td>';
                    });
                s += '</table>';                                                
            $('#results').html(s);
        })
        .fail(function(xhr) {
            alert("Could not find info on that Org Id or Name");
            console.log('error', xhr);
        }); 
    });

    $('#variety-search').on('click', () => {
        searchVarieties("variety-dropdown", "variety", "selectFirstVarietyFormRemainder")
    });
});

function selectApplicant(orgId)
{
    $("#selectCrop").show();
    $("#comp_ApplicantId").val(orgId);           
}

function SetStatesForCountry() 
{
    var country = $("#comp_CountryOfOrigin option:selected");

        if(country.text() === "United States")
        {
            showStates("USA");
        } else  if(country.text() === "Canada")
        {
            showStates("Canada");
        } else 
        {
            showStates("Outside");
        }

};

function showStates(country)
{   
    $("#comp_StateOfOrigin option").filter(function(index) { return $(this).text() === "Outside US"; }).attr('selected', 'selected');
    var selectText, indexText ;
    if(country === "USA")
    {   
        selectText = "Alabama, USA";
        indexText = ", USA";
    } else if(country === "Canada"){
        selectText = "Alberta, Canada";
        indexText = ", Canada";               
    } else {
        selectText = "Outside US";
        indexText = "Outside US";                   
    }
    
    $("#comp_StateOfOrigin > option").each(function() {
        if(~this.text.indexOf(indexText))
        {
            $("#comp_StateOfOrigin option[value*='" + this.value + "']").prop('disabled', false);
        } else {
            $("#comp_StateOfOrigin option[value*='" + this.value + "']").prop('disabled', true); 
        }
    });
        
    $("#comp_StateOfOrigin option").removeAttr("selected");                         
    $("#comp_CountryOfOrigin option:contains(" + selectText + ")").attr('selected', 'selected');
    
};

function searchVarieties(dropdownId, varietyInputId, selectVarietyCallback) {            
    let cropId = $("#comp_CropId").val();
    if (cropId === "") {
        alert("Warning: No crop selected. Please update and try again"); 
        return;
    }
    

    let varietyName = $("#Variety").val();
    if (varietyName === "") {
        alert("Warning: No variety found entered. Please update and try again"); 
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
        url: "/client/Application/FindVariety",
        data: data,
        success: function (res) {
            vs.innerHTML = "";
            // No varieties were found
            if (res.length === 0) {
                alert("Warning: No variety found. No variety with the entered name was found in our system. Please update and try again"); 
                //window[selectVarietyCallback](-1, varietyName);                      
            }
            // Populate dropdown with list of varieties
            res.forEach((el) => {
                vs.innerHTML += `<a class="dropdown-item" id=variety-${el.id} value=${el.id}>${el.name}</a>`;
                document.getElementById(`variety-${el.id}`).addEventListener('click', (e) => {
                    varietySelected(el.id);
                });
            })
        },
        error: function (res) {
            alert("There was an error processing the request");
        }
    });
}

function varietySelected(varietyId) {
    $("#completeForm").show();
    $("#Buttons").show();
    $("#comp_OfficialVarietyId").val(varietyId);
};