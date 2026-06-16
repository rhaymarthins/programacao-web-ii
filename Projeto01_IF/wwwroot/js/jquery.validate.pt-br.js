// Antonio Ray Martins Vieira

jQuery.extend(jQuery.validator.messages, {
    required: "Este campo é obrigatório.",
    email: "Informe um e-mail válido.",
    number: "Informe um número válido.",
    date: "Informe uma data válida.",
    maxlength: jQuery.validator.format("Máximo de {0} caracteres."),
    minlength: jQuery.validator.format("Mínimo de {0} caracteres.")
});

jQuery.extend(jQuery.validator.methods, {
    date: function (value, element) {
        return this.optional(element) || /^\d\d?\/\d\d?\/\d\d\d?\d?$/.test(value);
    },
    number: function (value, element) {
        return this.optional(element) || /^-?(?:\d+|\d{1,3}(?:\.\d{3})+)(?:,\d+)?$/.test(value);
    }
});