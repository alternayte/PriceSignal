import { Card, CardContent, CardHeader } from "@/components/ui/card";
import {Newspaper} from "lucide-react";
import { Link } from "react-router-dom";

export const MarketNewsList = () => {
    return (
        <div>
            <h2 className="text-xl font-bold mb-4 flex gap-x-2">Market News<Newspaper /></h2>
            <Card>
                <CardHeader className="flex flex-row items-center justify-between pb-2">
                </CardHeader>
                <CardContent>
                    <ul className="space-y-4">
                        {marketNews.map((news, index) => (
                            <li key={index} className='hover:bg-gray-50 relative'>
                                <div className="flex justify-between items-start">
                                    <div>
                                        <h3 className="text-lg font-medium text-blue-600">{news.title}</h3>
                                        <p className="text-muted-foreground line-clamp-1">{news.description}</p>
                                    </div>
                                    <Link to={news.url} className="text-primary">
                                        <span className="absolute inset-x-0 -top-px bottom-0"></span>
                                    </Link>
                                </div>
                            </li>
                        ))}
                    </ul>
                </CardContent>
            </Card>
        </div>
    );
};

const marketNews = [
    // {
    //     title: "Fed Raises Interest Rates Again",
    //     description: "The Federal Reserve has announced another rate hike to combat inflation.",
    //     url: "#",
    // },
    // {
    //     title: "Tech Stocks Rebound After Selloff",
    //     description: "Major tech companies see a surge in stock prices after a recent downturn.",
    //     url: "#",
    // },
    // {
    //     title: "Oil Prices Fluctuate Amid Global Tensions",
    //     description: "Geopolitical events continue to impact the volatile oil market.",
    //     url: "#",
    // },
    // {
    //     title: "Crypto Prices Stabilize After Volatility",
    //     description: "The cryptocurrency market shows signs of recovery following a period of uncertainty.",
    //     url: "#",
    // },
]