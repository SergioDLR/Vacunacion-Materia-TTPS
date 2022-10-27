const priceParcer = (number) => {
  return number.toLocaleString("es-ar", {
    style: "currency",
    currency: "ARS",
    minimumFractionDigits: 2,
  });
};

export default priceParcer;
