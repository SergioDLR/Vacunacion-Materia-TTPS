import axios from "axios";

export const cargarMarcas = (setMarcasComerciales, url, email, alert) => {
  try {
    axios
      .get(`${url}?emailOperadorNacional=${email}`)
      .then((response) => {
        if (response?.data?.estadoTransaccion === "Aceptada") {
          setMarcasComerciales(response?.data?.listasMarcasComercialesDTO);
        } else {
          alert.error(response?.data?.errores);
        }
      })
      .catch((error) => {
        alert.error("Ocurrio un error con el servidor: " + error);
      });
  } catch (e) {
    alert.error("Ocurrio un error con el servidor");
  }
};
