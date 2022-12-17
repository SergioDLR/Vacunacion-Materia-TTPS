import { Paper, SvgIcon } from "@mui/material";
import CancelIcon from "@mui/icons-material/Cancel";
const ItemCarrito = ({ item, handleDelete }) => {
  return (
    <Paper sx={{ marginTop: 1, marginBottom: 1, padding: 1 }}>
      {item.Vacuna.descripcion} - Cantidad: {item.CantidadVacunas}
      <SvgIcon sx={{ float: "right", color: "red", cursor: "pointer" }} onClick={() => handleDelete(item)}>
        <CancelIcon />
      </SvgIcon>
    </Paper>
  );
};

export default ItemCarrito;
