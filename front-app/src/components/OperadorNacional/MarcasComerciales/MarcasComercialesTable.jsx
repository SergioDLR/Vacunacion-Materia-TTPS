import { Table, TableContainer, TableHead, TableRow, TableCell, TableBody, Paper } from "@mui/material";
import MarcaComercial from "./MarcaComercial";
const MarcasComercialesTable = ({ marcasComerciales, cargarMarcasComerciales }) => {
  return (
    <TableContainer component={Paper}>
      <Table sx={{ minWidth: 650 }} size="small">
        <TableHead sx={{ backgroundColor: "#2E7994" }}>
          <TableRow>
            <TableCell sx={{ color: "white", fontWeight: 600 }}>Nombre</TableCell>
            <TableCell sx={{ color: "white", fontWeight: 600 }} align="right">
              Acciones
            </TableCell>
          </TableRow>
        </TableHead>
        <TableBody>
          {marcasComerciales.map((element, index) => (
            <MarcaComercial marcaComercial={element} key={index} cargarMarcasComerciales={cargarMarcasComerciales} />
          ))}
        </TableBody>
      </Table>
    </TableContainer>
  );
};

export default MarcasComercialesTable;
