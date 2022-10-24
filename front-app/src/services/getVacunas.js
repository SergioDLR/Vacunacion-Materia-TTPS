import axios from "axios";
export const cargarVacunas = (setVacunasCreadas, url, emailOp, alert, efectoSecundario = () => {}) => {
  try {
    axios
      .get(`${url}?emailOperadorNacional=${emailOp}`)
      .then((response) => {
        if (response?.data?.estadoTransaccion === "Aceptada") {
          setVacunasCreadas(response?.data?.listaVacunasDTO);
        } else {
          response?.data?.errores?.forEach((element) => alert.error(element));
        }
        efectoSecundario();
      })
      .catch((e) => {
        alert.error(e);
        efectoSecundario();
      });
  } catch (e) {
    alert.error("Ocurrio un error del lado del servidor");
  }
};
