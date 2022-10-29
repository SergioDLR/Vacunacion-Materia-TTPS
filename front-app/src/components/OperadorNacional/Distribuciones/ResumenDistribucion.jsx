import CustomButton from "@/components/utils/CustomButtom";
import { useState, useEffect } from "react";
import axios from "axios";
import allUrls from "@/services/backend_url";
import ItemResumen from "./ItemResumen";
import { Box } from "@mui/material";
const ResumenDistribucion = ({ resumen, email, jurisdiccion, alert, handleEnd, setOpen, cargarDistribuciones }) => {
  const [listaDetalles, setListaDetalles] = useState([]);
  useEffect(() => {
    crearSoloComprables();
  }, []);

  const crearSoloComprables = () => {
    resumen.listaSolicitudesEntregas.forEach((element) => {
      if (element.alertas.length === 0) {
        element.listaDetallesEntregas.forEach((lDetalle) => setListaDetalles([...listaDetalles, lDetalle]));
      }
    });
  };
  const handleCreate = () => {
    axios
      .post(allUrls.crearDistribucion, {
        EmailOperadorNacional: email,
        IdJurisdiccion: jurisdiccion,
        ListaDetallesEntregas: listaDetalles,
      })
      .then((response) => {
        if (response.data.estadoTransaccion === "Aceptada") {
          alert.success("Distribucion completada con exito");
          handleEnd();
          cargarDistribuciones();
        } else {
          alert.error(`Ocurrio un error`);
        }
      });
  };

  return (
    <>
      {resumen.listaSolicitudesEntregas.map((element, index) => (
        <ItemResumen item={element} key={index} />
      ))}
      <Box sx={{ marginTop: 1 }}>
        <CustomButton variant={"outlined"} color={"error"} textColor="red" onClick={() => setOpen(false)}>
          Cancelar
        </CustomButton>
        {listaDetalles.length > 0 && (
          <CustomButton variant={"outlined"} color={"info"} textColor="#2e7994" onClick={handleCreate}>
            Distribuir
          </CustomButton>
        )}
      </Box>
    </>
  );
};

export default ResumenDistribucion;
