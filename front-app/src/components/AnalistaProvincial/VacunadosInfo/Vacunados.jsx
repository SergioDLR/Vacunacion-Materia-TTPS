import axios from "axios";
import { useState, useEffect, useContext } from "react";
import { UserContext } from "../../Context/UserContext";
import allUrls from "@/services/backend_url";
import VacunadosTable from "./VacunadosTable";
import { Container } from "@mui/system";
import CustomLoader from "@/components/utils/CustomLoader";
import { useAlert } from "react-alert";
import { Box, InputLabel, MenuItem, Pagination, Select, TextField } from "@mui/material";
import CustomModal from "@/components/utils/Modal";
import CustomButton from "@/components/utils/CustomButtom";

const Vacunados = () => {
  const { userSesion } = useContext(UserContext);
  const [page, setPage] = useState(1);
  const [vacunados, setVacunados] = useState([]);
  const [estaCargando, setEstaCargando] = useState(true);
  const [cantPages, setCantPages] = useState(0);
  const [cantidadElementos, setCantidadElementos] = useState(10);
  const [cantVaM, setCantVaM] = useState(40000000);
  const [open, setOpen] = useState(false);
  const [loading, setLoading] = useState(false);
  const alert = useAlert();
  useEffect(() => {
    try {
      loadData(0, cantidadElementos);
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
    loadData(0, evt.target.value);
    calculatePages(evt.target.value);
  };

  const handleVacunacionMasiva = () => {
    let personaVacunada = [];
    let request = [];
    setLoading(true);

    for (let i = 40000000; i < 40000000 + 10; i++) {
      request.push(axios.get(`https://api.claudioraverta.com/personas/${i}`));
    }
    Promise.all(request).then((responses) => {
      responses.forEach((response) => {
        personaVacunada.push(response.data);
      });
      console.log(personaVacunada);
      axios
        .post(allUrls.vacunacionMasiva, { Email: userSesion.email, Usuarios: personaVacunada })
        .then((response) => {
          alert.success("Se vacunaron a los 1.000 con exito");
        })
        .catch(() => alert.error("Ocurrio un error"))
        .finally(() => setLoading(false));
    });
  };

  return (
    <Container>
      {estaCargando && <CustomLoader />}
      <div style={{ marginTop: 1 }}>
        <CustomModal title={"Vacunacion masiva"} textColor={"green"} color={"secondary"} open={open} setOpen={setOpen}>
          <form>
            <h4>¿Aplicar vacunacion masiva de 1000?</h4>
            <p>Se vacunara desde el dni 40.000.000 al 40.001.000</p>
            <CustomButton variant={"contained"} color={"error"} onClick={() => setOpen(false)}>
              Cancelar
            </CustomButton>
            <CustomButton color={"success"} variant={"contained"} onClick={handleVacunacionMasiva}>
              Confirmar
            </CustomButton>
          </form>
        </CustomModal>
        {loading && <CustomLoader />}
      </div>
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
