import axios from "axios";

const getVacunasDesarrolladas = (url, mail, setVacunasDesarrolladas, alert, setEstaCargando) => {
  try {
    axios
      .get(`${url}?emailOperadorNacional=${mail}`)
      .then((response) => {
        setVacunasDesarrolladas(response?.data?.listaVacunasDesarrolladasDTO);
      })
      .catch((error) => {
        alert.error(`Ocurrio un error ${error}`);
      })
      .finally(() => setEstaCargando(false));
  } catch (error) {
    alert.error(`Ocurrio un error del lado del servidor: ${error}`);
  }
};

export default getVacunasDesarrolladas;
