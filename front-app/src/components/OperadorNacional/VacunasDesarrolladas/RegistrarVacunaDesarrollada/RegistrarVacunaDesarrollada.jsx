import { useState, useEffect, useContext } from "react";
import { UserContext } from "@/components/Context/UserContext";
import { Button, TextField, InputLabel, Select, MenuItem, FormControl } from "@mui/material";
import axios from "axios";
import allUrls from "@/services/backend_url";
import { cargarVacunas } from "@/services/getVacunas";
import { useAlert } from "react-alert";
import { cargarMarcas } from "@/services/getMarcasComerciales";
const RegistrarVacunaDesarrollada = () => {
  const { userSesion } = useContext(UserContext);
  const [diasDemora, setDiasDemora] = useState(0);
  const [precio, setPrecio] = useState(0);
  const alert = useAlert();
  const [vacunasCreadas, setVacunasCreadas] = useState([]);
  const [vacunaSeleccionada, setVacunaSeleccionada] = useState(0);
  const [marcasComerciales, setMarcasComerciales] = useState([]);
  const [marcaSeleccionada, setMarcaSeleccionada] = useState(0);
  useEffect(() => {
    cargarMarcas(setMarcasComerciales, allUrls.marcasComerciales, userSesion.email, alert);
    cargarVacunas(setVacunasCreadas, allUrls.todasVacunas, userSesion.email, alert);
  }, []);

  const handleSubmit = (evt) => {
    evt.preventDefault();
    axios.post(allUrls.crearVacunaDesarrollada, {
      EmailOperadorNacional: userSesion.email,
      IdVacuna: vacunaSeleccionada,
      IdMarcaComercial: marcaSeleccionada,
      DiasDemoraEntrega: diasDemora,
      PrecioVacunaDesarrollada: precio,
    });
  };
  const handleChangeDiasDemora = (evt) => {
    setDiasDemora(evt.target.value);
  };
  const handleChangePrecio = (evt) => {
    setPrecio(evt.target.value);
  };
  const handleChangeVacuna = (evt) => {
    setVacunaSeleccionada(evt.target.value);
  };
  const handleChangeMarca = (evt) => {
    setMarcaSeleccionada(evt.target.value);
  };
  return (
    <form onSubmit={handleSubmit}>
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
            <MenuItem key={index} value={element.id}>
              {element.descripcionTipoVacuna} - {element.descripcion}
            </MenuItem>
          ))}
        </Select>
      </FormControl>
      <FormControl sx={{ marginTop: 1 }} fullWidth>
        <InputLabel id="Tipo-de-marca">Marcas comerciales:</InputLabel>
        <Select
          labelId="Tipo-de-marca"
          id="Tipo-de-marca-select"
          value={marcaSeleccionada}
          label="Marcas comerciales:"
          onChange={handleChangeMarca}
          required
        >
          <MenuItem disabled value={0}>
            Selecciona una marca comercial
          </MenuItem>
          {marcasComerciales.map((element, index) => (
            <MenuItem key={index} value={element.id}>
              {element.descripcion}
            </MenuItem>
          ))}
        </Select>
      </FormControl>
      <TextField
        sx={{ display: "block", marginTop: 1 }}
        fullWidth
        id="dias-field"
        label="Dias de demora"
        variant="filled"
        type="number"
        required
        inputProps={{ min: 0 }}
        onChange={handleChangeDiasDemora}
      />
      <TextField
        sx={{ display: "block", marginTop: 1 }}
        fullWidth
        id="precio-field"
        label="Precio"
        variant="filled"
        type="number"
        required
        onChange={handleChangePrecio}
        inputProps={{ min: 0 }}
      />
      <Button variant={"contained"} type={"submit"}>
        Crear
      </Button>
    </form>
  );
};

export default RegistrarVacunaDesarrollada;
