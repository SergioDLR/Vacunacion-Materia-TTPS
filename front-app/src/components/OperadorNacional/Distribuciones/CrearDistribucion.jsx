import CustomButton from "@/components/utils/CustomButtom";
import { FormControl, InputLabel, Select, MenuItem, Divider, Box } from "@mui/material";
import VacunaSelector from "./VacunaSelector";
import { UserContext } from "@/components/Context/UserContext";
import { useState, useContext } from "react";
import axios from "axios";
import ItemCarrito from "./ItemCarrito";
import CustomModal from "@/components/utils/Modal";
import allUrls from "@/services/backend_url";
import { useAlert } from "react-alert";
import ResumenDistribucion from "./ResumenDistribucion";
const CrearDistribucion = ({ jurisdicciones, setOpenMain, cargarDistribuciones }) => {
  const [idJurisdiccion, setIdJurisdiccion] = useState(0);
  const { userSesion } = useContext(UserContext);
  const [open, setOpen] = useState(false);
  const [carritoDeVacunas, setCarritoVacunas] = useState([]);
  const [responseConsulta, setResponseConsulta] = useState({});
  const [openAdv, setOpenAdv] = useState(false);
  const alert = useAlert();
  const handleChangeJurisdiccion = (evt) => {
    setIdJurisdiccion(evt.target.value);
  };

  const mapVacunes = () => {
    return carritoDeVacunas.map((element) => {
      return {
        IdVacuna: element.Vacuna.id,
        CantidadVacunas: element.CantidadVacunas,
      };
    });
  };

  const handleSubmit = (evt) => {
    evt.preventDefault();
    axios
      .post(allUrls.consultarDistribucion, {
        EmailOperadorNacional: userSesion.email,
        IdJurisdiccion: idJurisdiccion,
        ListaEnviosVacunas: mapVacunes(carritoDeVacunas),
      })
      .then((response) => {
        if (response.data.estadoTransaccion === "Aceptada") {
          setOpenAdv(true);
          setResponseConsulta(response.data);
        } else {
          alert.error("Se encontraron errores");
        }
      });
  };
  const handleEnd = () => {
    setOpen(false);
    setOpenAdv(false);
    setOpenMain(false);
  };
  const handleClose = () => {
    setOpen(false);
  };
  const handleAdd = (vacunaAgregar, cantidad) => {
    const find = carritoDeVacunas.find((element) => element.Vacuna.id === vacunaAgregar.id);
    const findIndex = carritoDeVacunas.findIndex((element) => element.Vacuna.id === vacunaAgregar.id);
    if (findIndex !== -1) {
      let newArr = [...carritoDeVacunas];
      newArr[findIndex] = {
        Vacuna: vacunaAgregar,
        CantidadVacunas: parseInt(cantidad) + parseInt(find.CantidadVacunas),
      };
      setCarritoVacunas(newArr);
    } else {
      setCarritoVacunas([...carritoDeVacunas, { Vacuna: vacunaAgregar, CantidadVacunas: cantidad }]);
    }
  };

  const handleDelete = (elementDelete) => {
    let newArr = carritoDeVacunas.filter((element) => element !== elementDelete);
    setCarritoVacunas(newArr);
  };
  return (
    <>
      <form onSubmit={handleSubmit}>
        <Box sx={{ marginTop: 1, marginBottom: 1 }}>
          <CustomModal open={open} setOpen={setOpen} title={"Agregar vacunas"}>
            <VacunaSelector userSesion={userSesion} handleClose={handleClose} handleAdd={handleAdd} />
          </CustomModal>
          {carritoDeVacunas.map((element, index) => (
            <ItemCarrito key={index} item={element} handleDelete={handleDelete} />
          ))}
        </Box>
        <Divider />
        <CustomModal open={openAdv} setOpen={setOpenAdv} displayButton={false}>
          <ResumenDistribucion
            resumen={responseConsulta}
            email={userSesion.email}
            jurisdiccion={idJurisdiccion}
            alert={alert}
            handleEnd={handleEnd}
            setOpen={setOpenAdv}
            cargarDistribuciones={cargarDistribuciones}
          />
        </CustomModal>
        <FormControl fullWidth sx={{ marginTop: 1 }}>
          <InputLabel id="input-jurisdiccion">Jurisdiccion</InputLabel>
          <Select
            labelId="input-jurisdiccion"
            id="input-jurisdiccion-select"
            onChange={handleChangeJurisdiccion}
            value={idJurisdiccion}
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
        </FormControl>
        <Box sx={{ marginTop: 1, marginBottom: 1 }}>
          <CustomButton variant={"outlined"} color={"error"} textColor={"red"} onClick={() => setOpenMain(false)}>
            Cancelar
          </CustomButton>
          {carritoDeVacunas.length > 0 && idJurisdiccion !== 0 && (
            <CustomButton type={"submit"} variant={"outlined"} color={"info"} textColor={"#2e7994"}>
              Crear
            </CustomButton>
          )}
        </Box>
      </form>
    </>
  );
};

export default CrearDistribucion;
