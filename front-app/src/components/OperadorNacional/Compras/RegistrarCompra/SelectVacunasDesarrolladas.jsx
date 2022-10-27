import axios from "axios";
import { useState, useContext, useEffect } from "react";
import { UserContext } from "@/components/Context/UserContext";
import getVacunasDesarrolladas from "@/services/getVacunasDesarrolladas";
import { FormControl, InputLabel, Select, MenuItem } from "@mui/material";
import { useAlert } from "react-alert";
const SelectVacunasDesarrolladas = ({
  urls,
  userMail,
  vacunaDesarrolladaSeleccionada,
  setVacunasDesarrolladasSeleccionada,
  efectoSecundario,
  opcionTodas = false,
}) => {
  const [vacunas, setVacunas] = useState([]);
  const [estaCargando, setEstaCargando] = useState(false);
  const alert = useAlert();
  useEffect(() => {
    getVacunasDesarrolladas(urls.todasVacunasDesarrolladas, userMail, setVacunas, alert, setEstaCargando);
  }, []);

  const handleChangeVacuna = (evt) => {
    setVacunasDesarrolladasSeleccionada(evt.target.value);
    if (efectoSecundario) efectoSecundario(evt.target.value);
  };
  return (
    <FormControl sx={{ marginTop: 1 }} fullWidth>
      <InputLabel id="Tipo-de-vacuna">Vacunas desarrollada:</InputLabel>
      <Select
        labelId="Tipo-de-vacuna"
        id="Tipo-de-vacuna"
        value={vacunaDesarrolladaSeleccionada}
        label="Vacunas desarrollada:"
        onChange={handleChangeVacuna}
        required
      >
        {opcionTodas ? (
          <MenuItem value={0}>Todas</MenuItem>
        ) : (
          <MenuItem disabled value={0}>
            Selecciona una vacuna desarrollada
          </MenuItem>
        )}
        {vacunas.map((element, index) => (
          <MenuItem key={index} value={element.id}>
            {element?.descripcionMarcaComercial} - {element?.descripcionVacuna} - Demora: {element?.diasDemoraEntrega}{" "}
            dias
          </MenuItem>
        ))}
      </Select>
    </FormControl>
  );
};

export default SelectVacunasDesarrolladas;
