import { Divider } from "@mui/material";

const ItemResumen = ({ item }) => {
  return (
    <>
      {item.alertas.length > 0 ? (
        <strong>Se encontraron problemas: {item.alertas}</strong>
      ) : (
        <>
          <strong>Se puede completar la orden: {item.listaDetallesEntregas[0].descripcionVacunaDesarrollada}</strong>
          <p>Cantidad: {item.cantidadEntrega}</p>
        </>
      )}
      <Divider />
    </>
  );
};

export default ItemResumen;
