import { Button } from '@/components/ui/button';
import { Command, CommandEmpty, CommandGroup, CommandInput, CommandItem } from '@/components/ui/command';
import { Check, ChevronsUpDownIcon, SearchIcon } from 'lucide-react';
import { Popover, PopoverContent, PopoverTrigger } from '@/components/ui/popover';
import React from 'react';
import { CommandList } from 'cmdk';
import { cn } from '@/lib/utils';
import { useNavigate } from 'react-router-dom';

const focusedSymbols = [
  { name: 'Bitcoin', symbol: 'BTCUSDT', token: 'BTC' },
  { name: 'Ethereum', symbol: 'ETHUSDT', token: 'ETH' },
  { name: 'Litecoin', symbol: 'LTCUSDT', token: 'LTC' },
  { name: 'Ripple', symbol: 'XRPUSDT', token: 'XRP' },
  { name: 'Dogecoin', symbol: 'DOGEUSDT', token: 'DOGE' },
];
export const SymbolsSearch = () => {
  const [open, setOpen] = React.useState(false);
  const [symbol, setSymbol] = React.useState('');

  const navigate = useNavigate();

  const onSymbolValueChange = (value: string) => {
    setSymbol(value);
    setOpen(false);
    navigate(`/symbols/${value}`);
    
  };
  return (
    <Popover open={open} onOpenChange={setOpen}>
      <PopoverTrigger asChild>
        <Button className="flex items-center gap-2" variant="outline">
          <SearchIcon className="h-4 w-4" />
          <span>{symbol || 'Search Coins'}</span>
          <ChevronsUpDownIcon className="h-4 w-4 ml-auto" />
        </Button>
      </PopoverTrigger>
      <PopoverContent className="w-[300px] p-4">
        <Command>
          <CommandInput className="h-9 w-full" placeholder="Search for a coin..." />
          <CommandEmpty>No coins found.</CommandEmpty>
          <CommandGroup>
            <CommandList>
              {focusedSymbols.map((value) => (
                <CommandItem key={value.symbol} value={value.symbol} onSelect={onSymbolValueChange}>
                  <Check className={cn('mr-2 h-4 w-4', value.symbol === symbol ? 'opacity-100' : 'opacity-0')} />
                  {value.name} ({value.token})
                </CommandItem>
              ))}
            </CommandList>
          </CommandGroup>
        </Command>
      </PopoverContent>
    </Popover>
  );
};
