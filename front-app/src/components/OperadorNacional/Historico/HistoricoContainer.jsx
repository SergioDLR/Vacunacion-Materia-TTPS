import { useState, useContext, useEffect } from "react";
import { UserContext } from "@/components/Context/UserContext";
import getVacunasDesarrolladas from "@/services/getVacunasDesarrolladas";
import allUrls from "@/services/backend_url";
import { useAlert } from "react-alert";
import { Box, Container } from "@mui/system";
import CustomLoader from "@/components/utils/CustomLoader";
import SelectVacunasDesarrolladas from "../Compras/RegistrarCompra/SelectVacunasDesarrolladas";
import { MenuItem, Select, TextField } from "@mui/material";
import axios from "axios";
import CustomButton from "@/components/utils/CustomButtom";

import Stepper from "@mui/material/Stepper";
import Step from "@mui/material/Step";
import StepLabel from "@mui/material/StepLabel";
import StepContent from "@mui/material/StepContent";
import Button from "@mui/material/Button";
import Paper from "@mui/material/Paper";
import Typography from "@mui/material/Typography";
import numberParser from "@/components/utils/numberParser";

const HistoricoContainer = () => {
  const { userSesion } = useContext(UserContext);
  const [vacunaDesarrolladaSeleccionada, setVacunasDesarrolladasSeleccionada] = useState(0);
  const [isLoading, setIsLoading] = useState(false);
  const [jurisdicciones, setJurisdicciones] = useState([]);
  const [jurisdiccionSeleccionada, setJurisdiccionSeleccionada] = useState(0);
  const [fechaDesde, setFechaDesde] = useState("10/07/2022");
  const [fechaHasta, setFechaHasta] = useState("18/12/2022");
  const [resultQuery, setResultQuery] = useState();

  const handleChangeJurisdiccion = (evt) => {
    setJurisdiccionSeleccionada(evt.target.value);
  };
  const alert = useAlert();
  useEffect(() => {
    axios.get(`${allUrls.jurisdiccion}GetAll`).then((response) => setJurisdicciones(response.data));
  }, []);

  const handleExecute = () => {
    setIsLoading(true);
    axios
      .get(
        `${allUrls.historico}?emailOperadorNacional=${userSesion.email}&idVacunaDesarrollada=${vacunaDesarrolladaSeleccionada}&idJurisdiccion=${jurisdiccionSeleccionada}&fechaDesde=${fechaDesde}&fechaHasta=${fechaHasta}`
      )
      .then((response) => {
        if (response.data.estadoTransaccion === "Aceptada") {
          if (response.data.detallesMesesAnios.length === 0) alert.error("No hay registros");
          else setResultQuery(response.data);
        } else {
          alert.error(response.data.errores);
        }
      })
      .finally(() => setIsLoading(false));
  };

  const handleSetFechaDesde = (evt) => {
    setFechaDesde(evt.target.value);
  };

  const handleSetFechaHasta = (evt) => {
    setFechaHasta(evt.target.value);
  };
  return (
    <Container>
      <Box sx={{ marginTop: 2 }}>
        {isLoading && <CustomLoader />}
        <SelectVacunasDesarrolladas
          urls={allUrls}
          userMail={userSesion.email}
          vacunaDesarrolladaSeleccionada={vacunaDesarrolladaSeleccionada}
          setVacunasDesarrolladasSeleccionada={setVacunasDesarrolladasSeleccionada}
          opcionTodas={false}
        />
        <Box sx={{ marginTop: 2 }}>
          <TextField
            id="date"
            label="Fecha desde"
            type="date"
            onChange={handleSetFechaDesde}
            defaultValue="2022-12-19"
            sx={{ width: 220 }}
            InputLabelProps={{
              shrink: true,
            }}
            fullWidth
          />
          <TextField
            id="date"
            label="Fecha hasta"
            type="date"
            defaultValue="2022-12-19"
            onChange={handleSetFechaHasta}
            sx={{ width: 220 }}
            InputLabelProps={{
              shrink: true,
            }}
            fullWidth
          />
          <Select
            labelId="input-jurisdiccion"
            id="input-jurisdiccion-select"
            onChange={handleChangeJurisdiccion}
            value={jurisdiccionSeleccionada}
            label="Jurisdiccion"
            defaultValue={1}
          >
            <MenuItem value={0} disabled>
              Selecciona una opcion
            </MenuItem>
            {jurisdicciones.map((element, index) => (
              <MenuItem value={element.id} key={index}>
                {element.descripcion}
              </MenuItem>
            ))}
          </Select>
          {vacunaDesarrolladaSeleccionada !== 0 && jurisdiccionSeleccionada !== 0 && (
            <CustomButton onClick={handleExecute} variant={"outlined"} color={"info"} textColor={"#2e7994"}>
              Buscar
            </CustomButton>
          )}
        </Box>
        {resultQuery && (
          <Box>
            <h2>Resultados de la busqueda:</h2>
            <h3>Cantidad de aplicaciones: {resultQuery?.totalAplicadas}</h3>
            <h3>Cantidad de vacunas disponibles: {resultQuery?.saldoTotal}</h3>

            <h3>Historico:</h3>
            <Stepper orientation="vertical">
              {resultQuery.detallesMesesAnios.map((step, index) => (
                <Step key={index}>
                  <StepLabel>
                    Mes: {step.numeroMes} - Año: {step.numeroAnio} - Cantidad de vacunados: {step.aplicadas}
                  </StepLabel>
                </Step>
              ))}
            </Stepper>
          </Box>
        )}
      </Box>
    </Container>
  );
};

export default HistoricoContainer;
