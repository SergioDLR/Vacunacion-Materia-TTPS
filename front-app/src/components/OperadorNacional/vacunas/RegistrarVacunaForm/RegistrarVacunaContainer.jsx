import { useState, useEffect, useContext } from "react";
import RegistrarVacuna from "../RegistraVacuna";
import SelectPandemia from "./SelectPandemia";
import SelectCalendario from "./SelectCalendario";
import axios from "axios";
import allUrls from "../../../../services/backend_url";
import { Button } from "@mui/material";
import { UserContext } from "../../../Context/UserContext";
import { useAlert } from "react-alert";
import SelectAnual from "./SelectAnual";
const RegistrarVacunaContainer = ({ cargarTodasLasVacunas, setOpen }) => {
  const [tipoVacunaSelected, setTipoVacunaSelected] = useState(0);
  const [descripcion, setDescripcion] = useState(0);
  const [optionSelectCalendario, setOptionSelectCalendario] = useState("");
  const { userSesion } = useContext(UserContext);
  const alert = useAlert();

  useEffect(() => {
    setDescripcion(0);
  }, [tipoVacunaSelected]);
  const handleSubmit = () => {
    try {
      axios
        .post(allUrls.crearVacuna, {
          EmailOperadorNacional: userSesion.email,
          IdTipoVacuna: tipoVacunaSelected,
          Descripcion: optionSelectCalendario,
          IdPandemia: descripcion,
        })
        .then((response) => {
          if (response?.data?.estadoTransaccion === "Aceptada") {
            alert.success("Se creo la vacuna con exito");
            cargarTodasLasVacunas();
          } else {
            response.data.errores.forEach((element) => {
              alert.error(element);
            });
          }
        })
        .catch((error) => alert.error(`Ocurrio un error del lado del servidor ${error}`))
        .finally(() => setOpen(false));
    } catch (e) {
      alert.error("Hay un error con el servidor");
    }
  };
  return (
    <>
      <RegistrarVacuna
        tipoVacunaSelected={tipoVacunaSelected}
        setTipoVacunaSelected={setTipoVacunaSelected}
        setDescripcion={setOptionSelectCalendario}
      />
      {tipoVacunaSelected === 1 && (
        <SelectCalendario descripcion={optionSelectCalendario} setDescripcion={setOptionSelectCalendario} />
      )}
      {tipoVacunaSelected === 2 && (
        <SelectAnual descripcion={optionSelectCalendario} setDescripcion={setOptionSelectCalendario} />
      )}
      {tipoVacunaSelected === 3 && (
        <SelectPandemia
          descripcion={descripcion}
          setDescripcion={setDescripcion}
          setDescriptionName={setOptionSelectCalendario}
        />
      )}
      <Button variant={"outlined"} color={"error"} onClick={() => setOpen(false)}>
        Cancelar
      </Button>
      {tipoVacunaSelected !== 0 && (descripcion !== 0 || optionSelectCalendario !== "") && (
        <Button variant={"contained"} onClick={handleSubmit}>
          Crear
        </Button>
      )}
    </>
  );
};

export default RegistrarVacunaContainer;
