import { useEffect, useState, useContext } from "react";
import { cargarVacunas } from "@/services/getVacunas";
import { useAlert } from "react-alert";
import allUrls from "@/services/backend_url";
import { FormControl, InputLabel, Select, MenuItem, ListItemText, ListItem, List, Divider, Paper } from "@mui/material";
import SelectDosis from "./SelectDosis";
import CustomLoader from "../utils/CustomLoader";
import CustomButton from "../utils/CustomButtom";
import CustomModal from "../utils/Modal";
import { UserContext } from "../Context/UserContext";
import { Box } from "@mui/material";
import axios from "axios";
import numberParser from "../utils/numberParser";
const FormularioVacunacion = ({ persona, email, setOpenPadre }) => {
  const alert = useAlert();
  const [vacunasCreadas, setVacunasCreadas] = useState([]);
  const [estaCargando, setEstaCargando] = useState(true);
  const [vacunaSeleccionada, setVacunaSeleccionada] = useState(0);
  const [dosisSeleccionada, setDosisSeleccionada] = useState(0);
  const [respuestaConsulta, setRespuestaConsulta] = useState({});
  const [open, setOpen] = useState(false);
  const [errores, setErrores] = useState([]);
  const { userSesion } = useContext(UserContext);

  useEffect(() => {
    cargarVacunas(setVacunasCreadas, allUrls.todasVacunas, email, alert, () => setEstaCargando(false));
  }, []);

  const handleChangeVacuna = (evt) => {
    setDosisSeleccionada(0);
    setVacunaSeleccionada(evt.target.value);
  };
  const dateParser = (str) => {
    return `${str[6]}${str[7]}${str[8]}${str[9]}-${str[3]}${str[4]}-${str[0]}${str[1]} T${str[11]}${str[12]}${str[13]}${str[14]}${str[15]}${str[16]}${str[17]}${str[18]}`;
  };

  const handleSubmit = (evt) => {
    evt.preventDefault();
    setEstaCargando(true);
    axios
      .post(allUrls.consultarVacunacion, {
        EmailVacunador: userSesion.email,
        Dni: persona.DNI,
        SexoBiologico: persona.genero,
        Nombre: persona.nombre,
        Apellido: persona.apellido,
        Embarazada: persona.embarazada,
        PersonalSalud: persona.personal_salud,
        FechaHoraNacimiento: dateParser(persona.fecha_hora_nacimiento),
        IdVacuna: vacunaSeleccionada.id,
        IdDosis: dosisSeleccionada.id,
        JurisdiccionResidencia: persona.jurisdiccion,
      })
      .then((response) => {
        if (response?.data?.estadoTransaccion === "Aceptada") {
          setRespuestaConsulta(response.data);
          if (response.data.alertasVacunacion !== null) {
            setErrores(response.data.alertasVacunacion);
          }
          setOpen(true);
        } else {
          alert.error(response.data.errores);
        }
      })
      .catch((error) => {
        console.log(error);
      })
      .finally(() => setEstaCargando(false));
  };

  const handleVacunacion = () => {
    try {
      setEstaCargando(true);
      axios
        .post(allUrls.crearVacunacion, {
          EmailVacunador: userSesion.email,
          Dni: persona.DNI,
          SexoBiologico: persona.genero,
          Nombre: persona.nombre,
          Apellido: persona.apellido,
          Embarazada: persona.embarazada,
          PersonalSalud: persona.personal_salud,
          FechaHoraNacimiento: dateParser(persona.fecha_hora_nacimiento),
          IdVacuna: respuestaConsulta.vacunaDesarrolladaAplicacion.IdVacuna,
          IdDosis: respuestaConsulta.dosisCorrespondienteAplicacion.id,
          JurisdiccionResidencia: persona.jurisdiccion,
          idLote: respuestaConsulta.vacunaDesarrolladaAplicacion.idLote,
          IdVacunaDesarrollada: respuestaConsulta.vacunaDesarrolladaAplicacion.id,
          Alertas: respuestaConsulta.alertasVacunacion.toString(),
          IdRegla: respuestaConsulta.dosisCorrespondienteAplicacion.reglas[0].id,
        })
        .then((response) => {
          if (response?.data?.estadoTransaccion === "Aceptada") {
            alert.success("La vacuna se aplico exitosamente");
            alert.success(response?.data?.vacunaDesarrolladaAplicacion.descripcion);
          } else {
            alert.error(response?.data?.alertasVacunacion);
          }
        })
        .catch((error) => {
          console.log(error);
        })
        .finally(() => {
          setOpenPadre(false);
          setEstaCargando(false);
        });
    } catch (e) {
      alert.error(`No hay stock para lo que se quiere aplicar`);
      setEstaCargando(false);
      setOpenPadre(false);
    }
  };
  return (
    <>
      {estaCargando && <CustomLoader />}
      <form onSubmit={handleSubmit}>
        <Paper sx={{ padding: 1 }}>
          <List component="nav" aria-label="mailbox folders">
            <ListItem>
              <ListItemText primary={`Nombre: ${persona.nombre}`} />
            </ListItem>
            <Divider variant={"fullWidth"} />
            <ListItem divider>
              <ListItemText primary={`Apellido: ${persona.apellido}`} />
            </ListItem>
            <ListItem>
              <ListItemText primary={`DNI: ${numberParser(persona.DNI)}`} />
            </ListItem>
            <Divider variant={"fullWidth"} />
            <ListItem>
              <ListItemText primary={`jurisdicción: ${persona.jurisdiccion}`} />
            </ListItem>
            <Divider variant={"fullWidth"} />
            {persona?.embarazada && (
              <>
                <ListItem>
                  <ListItemText primary={`Esta embarazada`} />
                </ListItem>
                <Divider variant={"fullWidth"} />
              </>
            )}
            {persona?.personal_salud && (
              <>
                <ListItem>
                  <ListItemText primary={`Es personal de salud`} />
                </ListItem>
                <Divider variant={"fullWidth"} />
              </>
            )}
            <ListItem>
              <ListItemText primary={`Nacimiento:  ${persona.fecha_hora_nacimiento}`} />
            </ListItem>
          </List>
        </Paper>
        <FormControl sx={{ marginTop: 1 }} fullWidth>
          <InputLabel id="Tipo-de-vacuna">Vacunas:</InputLabel>
          <Select
            labelId="Tipo-de-vacuna"
            id="Tipo-de-vacuna"
            value={vacunaSeleccionada}
            label="Vacunas:"
            onChange={handleChangeVacuna}
            required
          >
            <MenuItem disabled value={0}>
              Selecciona una vacuna
            </MenuItem>
            {vacunasCreadas.map((element, index) => (
              <MenuItem key={index} value={element}>
                {element.descripcionTipoVacuna} - {element.descripcion}
              </MenuItem>
            ))}
          </Select>
        </FormControl>
        {vacunaSeleccionada != 0 && (
          <SelectDosis
            vacuna={vacunaSeleccionada}
            dosisSeleccionada={dosisSeleccionada}
            setDosisSeleccionada={setDosisSeleccionada}
          />
        )}
        <Box>
          <CustomButton
            sx={{ marginTop: 1 }}
            type={"submit"}
            variant={"contained"}
            color={"error"}
            onClick={() => setOpenPadre(false)}
          >
            Cancelar
          </CustomButton>
          {vacunaSeleccionada != 0 && dosisSeleccionada != 0 && (
            <CustomButton sx={{ marginTop: 1 }} type={"submit"} variant={"outlined"} color={"info"} textColor={"black"}>
              Consultar
            </CustomButton>
          )}
        </Box>
      </form>
      <CustomModal displayButton={false} open={open} setOpen={setOpen}>
        {errores.length > 0 ? (
          <>
            <h2>Se encontraron problemas:</h2>
            <ul>
              {errores.map((error, index) => (
                <li key={index}>{error}</li>
              ))}
            </ul>
          </>
        ) : (
          <h2>No hay problemas para aplicar la vacuna</h2>
        )}
        {respuestaConsulta?.vacunaDesarrolladaAplicacion !== null && (
          <h5>Se aplicara la vacuna: {respuestaConsulta?.vacunaDesarrolladaAplicacion?.descripcion}</h5>
        )}
        {respuestaConsulta?.vacunaDesarrolladaAplicacion !== null && (
          <h5>Y la dosis: {respuestaConsulta?.dosisCorrespondienteAplicacion?.descripcion}</h5>
        )}
        {respuestaConsulta?.vacunaDesarrolladaAplicacion && errores.length < 1 === null && (
          <h4>No hay stock disponible</h4>
        )}
        <CustomButton
          sx={{ marginTop: 1 }}
          variant={"outlined"}
          color={"error"}
          textColor={"black"}
          onClick={() => setOpen(false)}
        >
          Cancelar
        </CustomButton>
        {respuestaConsulta?.vacunaDesarrolladaAplicacion !== null && (
          <CustomButton
            sx={{ marginTop: 1 }}
            variant={"outlined"}
            color={"info"}
            textColor={"black"}
            onClick={handleVacunacion}
          >
            Vacunar
          </CustomButton>
        )}
      </CustomModal>
    </>
  );
};

export default FormularioVacunacion;
