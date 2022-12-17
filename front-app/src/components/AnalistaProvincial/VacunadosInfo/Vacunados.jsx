import axios from "axios";
import { useState, useEffect, useContext } from "react";
import { UserContext } from "../../Context/UserContext";
import allUrls from "@/services/backend_url";
import VacunadosTable from "./VacunadosTable";
import { Container } from "@mui/system";
import CustomLoader from "@/components/utils/CustomLoader";
import { useAlert } from "react-alert";
import { Pagination } from "@mui/material";
const Vacunados = () => {
  const { userSesion } = useContext(UserContext);
  const [page, setPage] = useState(1);
  const [vacunados, setVacunados] = useState([]);
  const [estaCargando, setEstaCargando] = useState(true);
  const [cantPages, setCantPages] = useState(0);
  const alert = useAlert();
  useEffect(() => {
    try {
      loadData();
    } catch (e) {
      alert.error(`Ocurrio un error del lado del servidor`);
    }
  }, []);

  const handleChangePage = (evt, value) => {
    setEstaCargando(true);
    loadData();
    setPage(value);
  };

  const calculatePages = () => {
    //TODO: Tiene que calcular las paginas / 20
    return 50;
  };
  useEffect(() => {
    setCantPages(calculatePages());
  }, []);

  const loadData = () => {
    axios
      .get(`${allUrls.vacunasAplidas}?emailUsuario=${userSesion.email}`)
      .then((response) => {
        setEstaCargando(false);
        setVacunados(response?.data?.listaVacunasAplicadasDTO);
      })
      .catch((e) => console.log(e));
  };

  return (
    <Container>
      {estaCargando && <CustomLoader />}
      {/* TODO: Hay que agregar el filtrado por algo, no se que*/}
      <VacunadosTable vacunados={vacunados} />
      <Pagination count={cantPages} page={page} onChange={handleChangePage} siblingCount={5} />
    </Container>
  );
};

export default Vacunados;
