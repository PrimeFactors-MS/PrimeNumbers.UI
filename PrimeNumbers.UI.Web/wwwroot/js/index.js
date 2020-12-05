function calculatePrime() {
    var numberToCheck = parseInt(document.getElementById("primeNumber").value, 10)
    postData("Home/CheckPrime", { "Number": numberToCheck })
        .then(data => {
            var outputText = ""
            console.log(data)
            if (data.ErrorMessage) {
                outputText = "Got error: " + data.ErrorMessage
            } else if (data.IsPrime) {
                outputText = "Number is prime"
            } else {
                outputText = "Number is composed of: [" + data.Primes.toString() + "]"
            }
            document.getElementById("primeCalcResult").innerHTML = outputText
        })
}

async function postData(url = '', data = {}) {
    // Default options are marked with *
    const response = await fetch(url, {
        method: 'POST',
        mode: 'same-origin',
        cache: 'no-cache',
        credentials: 'same-origin',
        headers: {
            'Content-Type': 'application/json'
        },
        //redirect: 'follow', // manual, *follow, error
        referrerPolicy: 'no-referrer', // no-referrer, *no-referrer-when-downgrade, origin, origin-when-cross-origin, same-origin, strict-origin, strict-origin-when-cross-origin, unsafe-url
        body: JSON.stringify(data) // body data type must match "Content-Type" header
    });
    return response.json(); // parses JSON response into native JavaScript objects
}