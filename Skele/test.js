require('defaults.js')(1200);

TestData.generate(50, function (i) {
    return {
        Name: 'Steve #' + (i + 1),
        Email: 'x'
    };
});
