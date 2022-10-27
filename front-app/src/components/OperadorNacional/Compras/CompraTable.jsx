import { Table, TableContainer, TableHead, TableRow, TableCell, TableBody, Paper } from "@mui/material";
import Compra from "./Compra";
const CompraTabla = ({ compras }) => {
  return (
    <TableContainer component={Paper} sx={{ marginTop: 1 }}>
      <Table sx={{ minWidth: 650 }} size="small">
        <TableHead sx={{ backgroundColor: "#2E7994" }}>
          <TableRow>
            <TableCell sx={{ color: "white", fontWeight: 600 }} align="center">
              Vacuna
            </TableCell>
            <TableCell sx={{ color: "white", fontWeight: 600 }} align="center">
              Lote
            </TableCell>
            <TableCell sx={{ color: "white", fontWeight: 600 }} align="center">
              Estado
            </TableCell>
            <TableCell sx={{ color: "white", fontWeight: 600 }} align="center">
              Cantidad vacunas
            </TableCell>
            <TableCell sx={{ color: "white", fontWeight: 600 }} align="center">
              Fecha compra
            </TableCell>
            <TableCell sx={{ color: "white", fontWeight: 600 }} align="center">
              Fecha entrega
            </TableCell>
            <TableCell sx={{ color: "white", fontWeight: 600 }} align="center">
              Cantidad distribuida
            </TableCell>
            <TableCell sx={{ color: "white", fontWeight: 600 }} align="center">
              Cantidad vencidas
            </TableCell>
            <TableCell sx={{ color: "white", fontWeight: 600 }} align="center">
              Precio total
            </TableCell>
          </TableRow>
        </TableHead>
        <TableBody>
          {compras.map((element, index) => (
            <Compra compra={element} key={index} />
          ))}
        </TableBody>
      </Table>
    </TableContainer>
  );
};

export default CompraTabla;
