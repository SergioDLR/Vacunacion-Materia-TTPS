import SelectVacunasDesarrolladas from "../Compras/RegistrarCompra/SelectVacunasDesarrolladas";
import allUrls from "@/services/backend_url";
import { TextField, Box, FormControl, InputLabel, Select, MenuItem } from "@mui/material";
import { useState, useEffect } from "react";
import { cargarVacunas } from "@/services/getVacunas";
import { useAlert } from "react-alert";

import CustomButton from "@/components/utils/CustomButtom";
const VacunaSelector = ({ vacunasDisponibles, userSesion, handleClose, handleAdd }) => {
  const [vacunaSeleccionada, setVacunaSeleccionada] = useState(0);
  const [vacunas, setVacunas] = useState([]);
  const [cantidad, setCantidad] = useState(1000);
  const [cargando, setEstaCargando] = useState(false);
  const alert = useAlert();
  const handleChangeCantidad = (evt) => {
    setCantidad(evt.target.value);
  };
  const handleAddVacunas = () => {
    handleAdd(vacunaSeleccionada, cantidad);
    handleClose();
  };
  useEffect(() => {
    cargarVacunas(setVacunas, allUrls.todasVacunas, userSesion.email, alert, () => setEstaCargando(false));
  }, []);
  console.log(vacunas);
  const handleChangeVacuna = (evt) => {
    setVacunaSeleccionada(evt.target.value);
  };
  return (
    <>
      <FormControl sx={{ marginTop: 1 }} fullWidth>
        <InputLabel id="Tipo-de-vacuna">Vacunas desarrollada:</InputLabel>
        <Select
          labelId="Tipo-de-vacuna"
          id="Tipo-de-vacuna"
          value={vacunaSeleccionada}
          label="Vacunas desarrollada:"
          onChange={handleChangeVacuna}
          required
        >
          <MenuItem disabled value={0}>
            Selecciona una vacuna desarrollada
          </MenuItem>
          {vacunas.map((element, index) => (
            <MenuItem key={index} value={element}>
              {element?.descripcion}
            </MenuItem>
          ))}
        </Select>
      </FormControl>
      <TextField
        sx={{ display: "block", marginTop: 1 }}
        fullWidth
        id="cantidad-field"
        label="Cantidad:"
        variant="outlined"
        value={cantidad}
        type={"number"}
        inputProps={{ min: 1000, max: 100000 }}
        required
        onChange={handleChangeCantidad}
      />
      <Box sx={{ marginTop: 1, marginBottom: 1 }}>
        <CustomButton variant={"outlined"} color={"error"} textColor={"red"} onClick={handleClose}>
          Cancelar
        </CustomButton>
        {vacunaSeleccionada !== 0 && cantidad > 0 && (
          <CustomButton variant={"outlined"} color={"info"} textColor={"#2e7994"} onClick={handleAddVacunas}>
            Agregar
          </CustomButton>
        )}
      </Box>
    </>
  );
};

export default VacunaSelector;
