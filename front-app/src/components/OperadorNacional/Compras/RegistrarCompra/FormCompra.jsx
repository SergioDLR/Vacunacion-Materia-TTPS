import axios from "axios";
import { useState, useEffect, useContext } from "react";
import { UserContext } from "@/components/Context/UserContext";
import SelectVacunasDesarrolladas from "./SelectVacunasDesarrolladas";
import allUrls from "@/services/backend_url";
import { TextField } from "@mui/material";
import CustomButton from "@/components/utils/CustomButtom";
const FormCompra = ({ setOpen, alert, cargarCompras }) => {
  const [vacunasDesarrolladaSeleccionada, setVacunasDesarrolladasSeleccionada] = useState(0);
  const [cantidad, setCantidad] = useState(0);
  const { userSesion } = useContext(UserContext);
  const handleSubmit = (evt) => {
    evt.preventDefault();
    axios
      .post(allUrls.crearCompra, {
        EmailOperadorNacional: userSesion.email,
        IdVacunaDesarrollada: vacunasDesarrolladaSeleccionada,
        CantidadVacunas: cantidad,
      })
      .then((response) => {
        if (response?.data?.estadoTransaccion === "Aceptada") {
          alert.success("Se realizo la compra con exito");
          cargarCompras();
          setOpen(false);
        } else {
          alert.error(response.data.errores);
        }
      })
      .catch((e) => alert.error(`Ocurrieron errores: ${e}`));
  };
  const handleChange = (evt) => {
    setCantidad(evt.target.value);
  };
  return (
    <>
      <form onSubmit={handleSubmit}>
        <SelectVacunasDesarrolladas
          urls={allUrls}
          userMail={userSesion.email}
          setVacunasDesarrolladasSeleccionada={setVacunasDesarrolladasSeleccionada}
          vacunasDesarrolladaSeleccionada={vacunasDesarrolladaSeleccionada}
        />
        <TextField
          id="field-marca"
          sx={{ marginTop: 1, marginBottom: 1 }}
          label="Cantidad de vacunas:"
          value={cantidad}
          variant="outlined"
          fullWidth
          required
          type={"number"}
          inputProps={{ min: 1000 }}
          onChange={handleChange}
        />
        <CustomButton sx={{ marginRight: 1 }} color={"info"} textColor={"#2E7994"} variant={"outlined"} type={"submit"}>
          Comprar
        </CustomButton>
      </form>
    </>
  );
};

export default FormCompra;
