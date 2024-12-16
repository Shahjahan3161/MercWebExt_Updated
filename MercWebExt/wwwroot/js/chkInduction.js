$('#sendInduction').on('click', function () {

    var isCorrect = ValidateQuestion();
    var isValid = ValidateForm();
    
    // When all answers is correct, send email 
    if(isCorrect && isValid)
    {
        var lastName = document.getElementById("LastName").value;
        var firstName = document.getElementById("FirstName").value;
        var driverEmail = document.getElementById("DriverEmail").value;
        var driverMobile = document.getElementById("DriverMobile").value;
        var regoNumber = document.getElementById("RegoNumber").value;
        var choices = getChoices();

        var returnData = 
        {
            LastName:lastName,
            FirstName:firstName,
            DriverEmail:driverEmail,
            DriverMobile:driverMobile,
            RegoNumber:regoNumber,
            qr_1:choices[0],
            qr_2:choices[1],
            qr_3:choices[2],
            qr_4:choices[3],
            qr_5:choices[4],
            qr_6:choices[5],
            qr_7:choices[6],
            qr_8:choices[7],
            qr_9:choices[8],
            qr_10:choices[9],
            qr_11:choices[10],
            qr_12:choices[11],
            qr_13:choices[12],
            qr_14:choices[13],
            qr_15:choices[14],
            qr_16:choices[15]
        };
        
        $.ajax({
        type: "POST",
        url: "/Induction/CheckInduction",
        data: returnData,
        contentType: 'application/x-www-form-urlencoded; charset=utf-8',
        dataType: 'json'
        });
        
        swal({
              title: "Good job!",
              text: "You have successfully completed the induction. An email have been sent to " + driverEmail +
                  "\n Please bring this email with you when you are onsite. Thank you",
              icon: "success",
            }).then((value) => {location.reload(true)});
    }
    else
    {   
        if(!isCorrect)
        {
            swal({
              title: "Invalid Answers",
              text: "You have not answered all the questions correctly.\nPlease check all your answers and try again.",
              icon: "error",
            });
        }
        else if(!isValid)
        {
            swal({
              title: "Invalid Details",
              text: "You have entered an invalid Personal Details\nPlease check and enter again.",
              icon: "error",
            });
        }
    }
});

function ValidateForm()
{
    var returnVal = true;
    
    //Check Personal Details
    if( (document.getElementById("LastName").value == null) &&
        (document.getElementById("FirstName").value == null) &&
        (document.getElementById("DriverEmail").value == null) )
    {
        document.getElementById("LastName").focus();
        returnVal = false;
    }
    else if
     (!(/^\w+([\.-]?\w+)*@\w+([\.-]?\w+)*(\.\w{2,3})+$/.test(document.getElementById("DriverEmail").value)))
    {   
        document.getElementById("DriverEmail").focus();
        returnVal = false;
    }

    return returnVal;
        
};

function ValidateQuestion()
{
    var choices = getChoices();
    var answers = 
        ["True", "False", "True", "True", "True",
         "True", "False", "True", "False", "False",
         "True", "False", "True", "True", "False", "True"];
    var isCorrect = true;

    // Get Answers and Check the answer is correct
    for (var i=1; i<17; i++)
    {   
        if((choices[i-1] == answers[i-1]) && isCorrect)
        {
            isCorrect = true;
        }
        else
        {
            isCorrect = false;
            console.log("Wrong : No." + i);
        }
    }
    return isCorrect;
        
};

function getChoices()
{
    var choices = [];
    for (var i=1; i<17; i++)
    {   
        var qName = 'qr_' + i;

        var els = document.getElementsByName(qName);
        for (var j=0;j<els.length;j++){
            if ( els[j].checked ) {
                choices.push(els[j].value);
            }
        }
    }

    return choices;
};
