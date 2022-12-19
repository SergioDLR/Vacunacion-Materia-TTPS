import axios from "axios";
import { useState, useEffect, useContext } from "react";
import { UserContext } from "../../Context/UserContext";
import allUrls from "@/services/backend_url";
import VacunadosTable from "./VacunadosTable";
import { Container } from "@mui/system";
import CustomLoader from "@/components/utils/CustomLoader";
import { useAlert } from "react-alert";
import { InputLabel, MenuItem, Pagination, Select } from "@mui/material";
const Vacunados = () => {
  const { userSesion } = useContext(UserContext);
  const [page, setPage] = useState(1);
  const [vacunados, setVacunados] = useState([]);
  const [estaCargando, setEstaCargando] = useState(true);
  const [cantPages, setCantPages] = useState(0);
  const [cantidadElementos, setCantidadElementos] = useState(10);
  const alert = useAlert();
  useEffect(() => {
    try {
      loadData(1, cantidadElementos);
    } catch (e) {
      alert.error(`Ocurrio un error del lado del servidor`);
    }
  }, []);

  const handleChangePage = (evt, value) => {
    setEstaCargando(true);
    setPage(value);
    console.log(value);
    loadData(value * cantidadElementos - cantidadElementos, cantidadElementos);
  };

  const calculatePages = (value = 10) => {
    if (userSesion.descripcionJurisdiccion === "Nación")
      axios.get(allUrls.cantResultados).then((response) => setCantPages(parseInt(response.data / value) + 1));
    else
      axios
        .get(`${allUrls.cantResultados}?descripcionJurisdiccion=${userSesion.descripcionJurisdiccion}`)
        .then((response) => setCantPages(parseInt(response.data / value) + 1));
  };
  useEffect(() => {
    calculatePages();
  }, []);

  const loadData = (inicial, final) => {
    axios
      .get(`${allUrls.vacunasAplidas}?emailUsuario=${userSesion.email}&skip=${inicial}&take=${final}`)
      .then((response) => {
        setEstaCargando(false);
        setVacunados(response?.data?.listaVacunasAplicadasDTO);
      })
      .catch((e) => console.log(e));
  };
  const handleChangeQuantity = (evt) => {
    setCantidadElementos(evt.target.value);
    loadData(1, evt.target.value);
    calculatePages(evt.target.value);
  };
  return (
    <Container>
      {estaCargando && <CustomLoader />}
      {/* TODO: Hay que agregar el filtrado por algo, no se que*/}
      <VacunadosTable vacunados={vacunados} />
      <Pagination count={cantPages} page={page} onChange={handleChangePage} siblingCount={5} />
      <InputLabel id="demo-simple-select-label">Cantidad de elementos por pagina</InputLabel>
      <Select
        labelId="demo-simple-select-label"
        id="demo-simple-select"
        value={cantidadElementos}
        label="Cantidad de elementos por pagina"
        onChange={handleChangeQuantity}
      >
        <MenuItem value={10}>10</MenuItem>
        <MenuItem value={20}>20</MenuItem>
        <MenuItem value={30}>30</MenuItem>
        <MenuItem value={40}>40</MenuItem>
        <MenuItem value={50}>50</MenuItem>
        <MenuItem value={60}>60</MenuItem>
      </Select>
    </Container>
  );
};

export default Vacunados;
