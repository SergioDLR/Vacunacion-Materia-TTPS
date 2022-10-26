import axios from "axios";
import { useState, useEffect, useContext } from "react";
import { UserContext } from "../../Context/UserContext";
import allUrls from "@/services/backend_url";
import VacunadosTable from "./VacunadosTable";
import { Container } from "@mui/system";
import CustomLoader from "@/components/utils/CustomLoader";

const Vacunados = () => {
  const { userSesion } = useContext(UserContext);
  const [vacunados, setVacunados] = useState([]);
  const [estaCargando, setEstaCargando] = useState(true);
  useEffect(() => {
    axios
      .get(`${allUrls.vacunasAplidas}?emailUsuario=${userSesion.email}`)
      .then((response) => {
        setEstaCargando(false);
        setVacunados(response?.data?.listaVacunasAplicadasDTO);
      })
      .catch(console.log("Ocurrio un error"));
  }, []);
  return (
    <Container>
      {estaCargando && <CustomLoader />}
      <VacunadosTable vacunados={vacunados} />
    </Container>
  );
};

export default Vacunados;
